using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(ISaleRepository saleRepository, IProductRepository productRepository, IBranchRepository branchRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existing == null)
            throw new InvalidOperationException($"Sale with ID not found");

        if (existing.SaleStatus == SaleStatusEnum.Cancelled.ToString())
            throw new InvalidOperationException($"Sale with ID {command.Id} is already canceled");

        if (string.IsNullOrWhiteSpace(command.SaleStatus))
        {
            command.SaleStatus = existing.SaleStatus;
        }
        if (command.BranchSaleId == null || command.BranchSaleId == Guid.Empty)
        {
            command.BranchSaleId = existing.BranchSaleId;
        }

        var products = await _productRepository.ListByIdsAsync(command.SaleItems.Select(x => x.ProductId).ToArray(), cancellationToken);
        if (products == null || !products.Any() || command.SaleItems.ToList().Count != products.ToList().Count)
            throw new KeyNotFoundException($"Product with ID not found");

        var branch = await _branchRepository.GetByIdAsync(command.BranchSaleId, cancellationToken);
        if (branch == null)
            throw new KeyNotFoundException($"Branch with ID not found");

        var sale = _mapper.Map<Sale>(command);
        sale.UpdateSale();

        var simulateSaleService = new SimulateSaleService(sale, products);
        var simulatedSale = simulateSaleService.MakePriceSimulation();

        simulatedSale.SetSaleNumber(existing.SaleNumber);
        simulatedSale.SaleDate = existing.SaleDate;
        simulatedSale.UserId = existing.UserId;

        var updated = await _saleRepository.UpdateAsync(simulatedSale, cancellationToken);
        var result = _mapper.Map<UpdateSaleResult>(updated);

        return result;
    }
}
