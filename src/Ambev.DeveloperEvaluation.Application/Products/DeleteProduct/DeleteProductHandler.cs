using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IQueryDatabaseService _queryDbService;

        public DeleteProductHandler(IProductRepository productRepository, IQueryDatabaseService queryDbService)
        {
            _productRepository = productRepository;
            _queryDbService = queryDbService;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existing == null)
                throw new KeyNotFoundException($"Product with ID {command.Id} not found");

            var hasProductInItems = await _queryDbService.ProductsInItem(command.Id);
            if (hasProductInItems)
                throw new InvalidOperationException($"Product with ID {command.Id} cannot be deleted because it is referenced in sales items.");

            var success = await _productRepository.DeleteAsync(command.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Product with ID {command.Id} not found");

            return new DeleteProductResult { Success = true };
        }
    }
}
