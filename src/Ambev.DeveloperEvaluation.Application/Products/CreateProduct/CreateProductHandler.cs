using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductHandler : IRequestApplicationHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDomainNotificationAdapter _notification;

        public CreateProductHandler(IProductRepository productRepository, IMapper mapper, IDomainNotificationAdapter notification)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _notification = notification;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = _mapper.Map<Product>(command);

            var created = await _productRepository.CreateAsync(product, cancellationToken);
            var productEvent = created.CreateProductCreatedEvent();

            var result = _mapper.Map<CreateProductResult>(created);
            await _notification.Publish(productEvent, cancellationToken);
            
            return result;
        }
    }
}
