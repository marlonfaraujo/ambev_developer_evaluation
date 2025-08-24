using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch
{
    public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, DeleteBranchResult>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IQueryDatabaseService _queryDbService;

        public DeleteBranchHandler(IBranchRepository branchRepository, IQueryDatabaseService queryDbService)
        {
            _branchRepository = branchRepository;
            _queryDbService = queryDbService;
        }

        public async Task<DeleteBranchResult> Handle(DeleteBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteBranchCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var success = await _branchRepository.DeleteAsync(command.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Branch with ID {command.Id} not found");

            var existing = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existing == null)
                throw new KeyNotFoundException($"Branch with ID {command.Id} not found");

            var hasBranchInItems = await _queryDbService.BranchsInItem(command.Id.ToString());
            if (hasBranchInItems)
                throw new InvalidOperationException($"Branch with ID {command.Id} cannot be deleted because it is referenced in sales");

            return new DeleteBranchResult { Success = true };
        }
    }
}
