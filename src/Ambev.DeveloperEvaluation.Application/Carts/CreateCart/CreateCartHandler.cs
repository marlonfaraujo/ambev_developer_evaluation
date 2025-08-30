using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart
{
    public class CreateCartHandler : IRequestApplicationHandler<CreateCartCommand, CreateCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public CreateCartHandler(ICartRepository cartRepository, IProductRepository productRepository, IBranchRepository branchRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateCartCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var products = await GetProductsById();
            var branch = await GetBranchById();

            var cart = _mapper.Map<Cart>(command);
            cart.BranchName = branch.Name;
            cart.CartItems.ToList().ForEach(x =>
            {
                var product = products.First(p => p.Id == x.ProductId);
                x.ProductName = product.Name;
            });

            var created = await _cartRepository.CreateAsync(cart, cancellationToken);
            var result = _mapper.Map<CreateCartResult>(created);
            return result;

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
