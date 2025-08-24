using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch
{
    public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, DeleteBranchResult>
    {
        private readonly IBranchRepository _branchRepository;

        public DeleteBranchHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<DeleteBranchResult> Handle(DeleteBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteBranchCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var success = await _branchRepository.DeleteAsync(command.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Record with ID {command.Id} not found");

            return new DeleteBranchResult { Success = true };
        }
    }
}
