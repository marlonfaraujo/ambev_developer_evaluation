using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductHandler : IRequestApplicationHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IQueryDatabaseService _queryDbService;
        private readonly ICartRepository _cartRepository;
        private readonly IDomainNotificationAdapter _notification;

        public DeleteProductHandler(IProductRepository productRepository, IQueryDatabaseService queryDbService, ICartRepository cartRepository, IDomainNotificationAdapter notification)
        {
            _productRepository = productRepository;
            _queryDbService = queryDbService;
            _cartRepository = cartRepository;
            _notification = notification;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await existingProductById();
            await validateProductIdInSaleItems();
            await validateProductIdInCartItems();

            var success = await _productRepository.DeleteAsync(command.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Product with ID {command.Id} not found");

            var productEvent = existing.CreateProductDeletedEvent();
            await _notification.Publish(productEvent, cancellationToken);
            return new DeleteProductResult { Success = true };

            async Task<Product> existingProductById()
            {
                var existing = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing == null)
                    throw new KeyNotFoundException($"Product with ID {command.Id} not found");
                return existing;
            }
            async Task validateProductIdInSaleItems()
            {
                var hasProductInItems = await _queryDbService.ProductInSaleItems(command.Id);
                if (hasProductInItems)
                    throw new InvalidOperationException($"Product with ID {command.Id} cannot be deleted because it is referenced in sales items.");
            }

            async Task validateProductIdInCartItems()
            {
                var filters = new Dictionary<string, object>();
                var options = new QueryOptions();
                filters.Add("CartItems.ProductId", command.Id);
                options.Filters = filters;

                var carts = await _cartRepository.GetPagedAsync(options, cancellationToken);
                if (carts != null && carts.Items.Any())
                {
                    throw new InvalidOperationException($"Product with ID {command.Id} cannot be deleted because it is referenced in cart items.");
                }
            }
        }
    }
}
