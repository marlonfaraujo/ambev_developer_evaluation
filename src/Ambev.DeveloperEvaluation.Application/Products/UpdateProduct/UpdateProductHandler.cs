using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestApplicationHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            
            await existingProductById();
            
            var product = _mapper.Map<Product>(command);
            var created = await _productRepository.UpdateAsync(product, cancellationToken);
            var result = _mapper.Map<UpdateProductResult>(created);
            return result;

            async Task existingProductById()
            {
                var existing = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing == null)
                    throw new InvalidOperationException($"Product ID not found");
            }
        }
    }
}
