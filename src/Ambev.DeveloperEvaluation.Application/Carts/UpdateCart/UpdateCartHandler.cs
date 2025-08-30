using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart
{
    public class UpdateCartHandler : IRequestApplicationHandler<UpdateCartCommand, UpdateCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public UpdateCartHandler(ICartRepository cartRepository, IProductRepository productRepository, IBranchRepository branchRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateCartCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingCart = await existingCartById();
            validateUserId();

            var products = await GetProductsById();
            var branch = await GetBranchById();

            var cart = _mapper.Map<Cart>(command);
            cart.UserId = existingCart.UserId;
            cart.BranchSaleId = branch.Id;
            cart.BranchName = branch.Name;

            cart.CartItems.ToList().ForEach(x =>
            {
                var product = products.First(p => p.Id == x.ProductId);
                x.ProductName = product.Name;
            });

            var updated = await _cartRepository.UpdateAsync(cart, cancellationToken);
            var result = _mapper.Map<UpdateCartResult>(updated);
            return result;

            void validateUserId()
            {
                if (existingCart.UserId != command.UserId)
                    throw new UnauthorizedAccessException($"You do not have permission to update this cart");
            }

            async Task<Cart> existingCartById()
            {
                var existing = await _cartRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing == null)
                    throw new KeyNotFoundException($"Cart with ID {command.Id} not found");
                return existing;
            }

            async Task<IEnumerable<Product>> GetProductsById()
            {
                var products = await _productRepository.ListByIdsAsync(command.CartItems.Select(x => x.ProductId).ToArray(), cancellationToken);
                if (products == null || !products.Any() || command.CartItems.ToList().Count != products.ToList().Count)
                    throw new KeyNotFoundException($"Product with ID not found");
                return products;
            }
            async Task<Branch> GetBranchById()
            {
                var branch = await _branchRepository.GetByIdAsync(command.BranchSaleId, cancellationToken);
                if (branch == null)
                    throw new KeyNotFoundException($"Branch not found");
                return branch;
            }
        }
    }
}
