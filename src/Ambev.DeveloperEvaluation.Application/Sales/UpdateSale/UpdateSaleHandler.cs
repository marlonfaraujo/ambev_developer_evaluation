using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestApplicationHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly IDomainNotificationAdapter _notification;

    public UpdateSaleHandler(ISaleRepository saleRepository, IProductRepository productRepository, IBranchRepository branchRepository, IMapper mapper, IDomainNotificationAdapter notification)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
        _notification = notification;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await existingSaleById();
        if (existingSale.SaleStatus == SaleStatusEnum.Cancelled.ToString())
            throw new InvalidOperationException($"Sale with ID {command.Id} is already canceled");

        if (string.IsNullOrWhiteSpace(command.SaleStatus))
        {
            command.SaleStatus = existingSale.SaleStatus;
        }
        if (command.BranchSaleId == null || command.BranchSaleId == Guid.Empty)
        {
            command.BranchSaleId = existingSale.BranchSaleId;
        }
        var products = await GetProductsById();
        await hasBranchById();

        var sale = _mapper.Map<Sale>(command);
        sale.UpdateSale();
        var simulateSaleService = new SimulateSaleService(sale, products);
        var simulatedSale = simulateSaleService.MakePriceSimulation();
        simulatedSale.SetSaleNumber(existingSale.SaleNumber);
        simulatedSale.SaleDate = existingSale.SaleDate;
        simulatedSale.UserId = existingSale.UserId;
        var saleEvent = simulatedSale.UpdateSale();
        var updated = await _saleRepository.UpdateAsync(simulatedSale, cancellationToken);
        var result = _mapper.Map<UpdateSaleResult>(updated);
        _notification.Publish(saleEvent, cancellationToken);
        return result;

        async Task<Sale> existingSaleById()
        {
            var existing = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existing == null)
                throw new InvalidOperationException($"Sale with ID not found");
            return existing;
        }
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
                throw new KeyNotFoundException($"Branch with ID not found");
        }
    }
}
