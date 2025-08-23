using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public CreateSaleHandler(ISaleRepository saleRepository, IProductRepository productRepository, IBranchRepository branchRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var products = await _productRepository.ListByIdsAsync(command.SaleItems.Select(x => x.ProductId).ToArray(), cancellationToken);
        if (products == null || !products.Any() || command.SaleItems.ToList().Count != products.ToList().Count)
            throw new KeyNotFoundException($"Product with ID not found");

        var branch = await _branchRepository.GetByIdAsync(command.BranchSaleId, cancellationToken);
        if (branch == null)
            throw new KeyNotFoundException($"Branch not found");

        var cacheSale = _mapper.Map<Sale>(command);

        var simulateSaleService = new SimulateSaleService(_mapper.Map<Sale>(command), products);
        var simulatedSale = simulateSaleService.MakePriceSimulation();

        if (cacheSale.TotalSalePrice != simulatedSale.TotalSalePrice)
        {
            throw new PriceProductsDifferentException($"The price of the products in the cart and the new price are different, delete cart and try again");
        }

        var created = await _saleRepository.CreateAsync(simulatedSale, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(created);
        return result;
    }
}
