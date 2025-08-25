using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleHandler : IRequestApplicationHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IDomainNotificationAdapter _notification;

        public CancelSaleHandler(ISaleRepository saleRepository, IMapper mapper, IDomainNotificationAdapter notification)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _notification = notification;
        }

        public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CancelSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existing == null)
                throw new InvalidOperationException($"Sale with ID not found");

            if (existing.SaleStatus == SaleStatusEnum.Cancelled.ToString())
                throw new InvalidOperationException($"Sale with ID {command.Id} is already canceled");

            var saleEvent = existing.CancelSale();
            var saleItemsEvent = existing.CancelSaleItems();
            var result = await _saleRepository.UpdateAsync(existing, cancellationToken);

            _notification.Publish(saleEvent, cancellationToken);
            foreach (var item in saleItemsEvent)
            {
                _notification.Publish(item, cancellationToken);
            }

            return _mapper.Map<CancelSaleResult>(result);
        }
    }
}
