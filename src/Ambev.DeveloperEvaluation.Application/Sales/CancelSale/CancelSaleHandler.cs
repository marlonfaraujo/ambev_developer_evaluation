using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public CancelSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
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

            var sale = _mapper.Map<Sale>(existing);

            sale.CancelSale();
            var result = await _saleRepository.UpdateAsync(sale, cancellationToken);

            return _mapper.Map<CancelSaleResult>(result);
        }
    }
}
