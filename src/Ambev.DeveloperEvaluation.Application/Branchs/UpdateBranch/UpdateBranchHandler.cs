using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch
{
    public class UpdateBranchHandler : IRequestApplicationHandler<UpdateBranchCommand, UpdateBranchResult>
    {

        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;
        private readonly IDomainNotificationAdapter _notification;

        public UpdateBranchHandler(IBranchRepository branchRepository, IMapper mapper, IDomainNotificationAdapter notification)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
            _notification = notification;
        }

        public async Task<UpdateBranchResult> Handle(UpdateBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateBranchCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await existingBranchById();

            var branch = _mapper.Map<Branch>(command);
            var updated = await _branchRepository.UpdateAsync(branch, cancellationToken);
            var branchEvent = updated.CreateBranchChangedEvent();
            var result = _mapper.Map<UpdateBranchResult>(updated);
            await _notification.Publish(branchEvent, cancellationToken);
            return result;

            async Task existingBranchById()
            {
                var existing = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing == null)
                    throw new InvalidOperationException($"Branch ID not found");
            }
        }
    }
}
