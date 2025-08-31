using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestApplicationHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDomainNotificationAdapter _notification;
        private readonly ICartRepository _cartRepository;

        public UpdateProductHandler(IProductRepository productRepository, IMapper mapper, IDomainNotificationAdapter notification, ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _notification = notification;
            _cartRepository = cartRepository;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await existingProductById();
            await validateChangePriceProduct();

            var product = _mapper.Map<Product>(command);
            var updated = await _productRepository.UpdateAsync(product, cancellationToken);
            var productEvent = updated.CreateProductChangedEvent();
            var result = _mapper.Map<UpdateProductResult>(updated);
            _notification.Publish(productEvent, cancellationToken);
            return result;

            async Task<Product> existingProductById()
            {
                var existing = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing == null)
                    throw new InvalidOperationException($"Product ID not found");

                return existing;
            }

            async Task validateChangePriceProduct()
            {
                if (command.Price == existing.Price) return;

                var carts = await GetCartsWithProductId(existing.Id);
                if (carts != null && carts.Any())
                {
                    throw new InvalidOperationException($"Cannot change the price of a product that is in an open cart");
                }
            }

            async Task<IEnumerable<Cart>> GetCartsWithProductId(Guid productId)
            {
                var filters = new Dictionary<string, object>();
                var options = new QueryOptions();
                filters.Add("CartItems.ProductId", productId);
                filters.Add("CartStatus", CartStatusEnum.Opened.ToString());
                options.Filters = filters;

                var carts = await _cartRepository.GetPagedAsync(options, cancellationToken);
                if (carts != null && carts.Items.Any())
                {
                    return carts.Items;
                }
                return Enumerable.Empty<Cart>();
            }
        }
    }
}
