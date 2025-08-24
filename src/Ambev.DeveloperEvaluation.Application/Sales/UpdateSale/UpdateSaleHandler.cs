using Ambev.DeveloperEvaluation.Domain.Entities;
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
            throw new InvalidOperationException($"Record with ID not found");

        var products = await _productRepository.ListByIdsAsync(command.SaleItems.Select(x => x.ProductId).ToArray(), cancellationToken);
        if (products == null || !products.Any() || command.SaleItems.ToList().Count != products.ToList().Count)
            throw new KeyNotFoundException($"Product with ID not found");

        var branch = await _branchRepository.GetByIdAsync(command.BranchSaleId, cancellationToken);
        if (branch == null)
            throw new KeyNotFoundException($"Branch not found");

        var sale = _mapper.Map<Sale>(command);

        var simulateSaleService = new SimulateSaleService(sale, products);
        var simulatedSale = simulateSaleService.MakePriceSimulation();

        var created = await _saleRepository.CreateAsync(simulatedSale, cancellationToken);
        var result = _mapper.Map<UpdateSaleResult>(created);

        sale.UpdateSale();
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return result;
    }
}
