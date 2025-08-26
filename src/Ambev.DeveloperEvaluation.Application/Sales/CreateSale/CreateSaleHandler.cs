using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestApplicationHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly IDomainNotificationAdapter _notification;

    public CreateSaleHandler(ISaleRepository saleRepository, IProductRepository productRepository, IBranchRepository branchRepository, IMapper mapper, IDomainNotificationAdapter notification)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
        _notification = notification;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var products = await GetProductsById();
        await hasBranchById();

        var cacheSale = _mapper.Map<Sale>(command);
        var simulateSaleService = new SimulateSaleService(_mapper.Map<Sale>(command), products);
        var simulatedSale = simulateSaleService.MakePriceSimulation();
        if (cacheSale.TotalSalePrice != simulatedSale.TotalSalePrice)
        {
            throw new PriceProductsDifferentException($"The price of the products in the cart and the new price are different, delete cart and try again");
        }

        var created = await _saleRepository.CreateAsync(simulatedSale, cancellationToken);
        var saleEvent = created.CreateSaleEvent();
        var result = _mapper.Map<CreateSaleResult>(created);
        _notification.Publish(saleEvent, cancellationToken);

        return result;

        async Task<IEnumerable<Product>> GetProductsById()
        {
            var products = await _productRepository.ListByIdsAsync(command.SaleItems.Select(x => x.ProductId).ToArray(), cancellationToken);
            if (products == null || !products.Any() || command.SaleItems.ToList().Count != products.ToList().Count)
                throw new KeyNotFoundException($"Product with ID not found");
            return products;
        }
        async Task hasBranchById()
        {
            var branch = await _branchRepository.GetByIdAsync(command.BranchSaleId, cancellationToken);
            if (branch == null)
                throw new KeyNotFoundException($"Branch not found");
        }
    }
}
