using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleHandler : IRequestHandler<SimulateSaleQuery, SimulateSaleResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public SimulateSaleHandler(IProductRepository productRepository, IBranchRepository branchRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<SimulateSaleResult> Handle(SimulateSaleQuery query, CancellationToken cancellationToken)
        {
            var validator = new SimulateSaleQueryValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var products = await _productRepository.ListByIdsAsync(query.SaleItems.Select(x => x.ProductId).ToArray(), cancellationToken);
            if (products == null || !products.Any() || query.SaleItems.ToList().Count != products.ToList().Count)
                throw new KeyNotFoundException($"Product with ID not found");

            var branch = await _branchRepository.GetByIdAsync(query.BranchSaleId, cancellationToken);
            if (branch == null)
                throw new KeyNotFoundException($"Branch not found");

            if (query.SaleItems.Any(x => x.ProductItemQuantity <= 0))
            {
                throw new KeyNotFoundException($"Quantity of product less than 1");
            }

            var simulateSaleService = new SimulateSaleService(_mapper.Map<Sale>(query), products);
            var sale = simulateSaleService.MakePriceSimulation();

            return _mapper.Map<SimulateSaleResult>(sale);
        }
    }
}
