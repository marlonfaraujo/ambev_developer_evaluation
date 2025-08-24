using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.SimulateSale
{
    public class SimulateSaleHandler : IRequestHandler<SimulateSaleCommand, SimulateSaleResult>
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

        public async Task<SimulateSaleResult> Handle(SimulateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new SimulateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var products = await _productRepository.ListByIdsAsync(command.SaleItems.Select(x => x.ProductId).ToArray(), cancellationToken);
            if (products == null || !products.Any() || command.SaleItems.ToList().Count != products.ToList().Count)
                throw new KeyNotFoundException($"Product with ID not found");

            var branch = await _branchRepository.GetByIdAsync(command.BranchSaleId, cancellationToken);
            if (branch == null)
                throw new KeyNotFoundException($"Branch not found");

            var simulateSaleService = new SimulateSaleService(_mapper.Map<Sale>(command), products);
            var sale = simulateSaleService.MakePriceSimulation();

            return _mapper.Map<SimulateSaleResult>(sale);
        }
    }
}
