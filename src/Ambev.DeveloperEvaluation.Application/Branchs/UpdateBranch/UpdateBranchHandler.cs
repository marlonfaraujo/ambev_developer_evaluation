using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch
{
    public class UpdateBranchHandler : IRequestApplicationHandler<UpdateBranchCommand, UpdateBranchResult>
    {

        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public UpdateBranchHandler(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<UpdateBranchResult> Handle(UpdateBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateBranchCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existing = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existing == null)
                throw new InvalidOperationException($"Branch ID not found");

            var branch = _mapper.Map<Branch>(command);

            var updated = await _branchRepository.UpdateAsync(branch, cancellationToken);
            var result = _mapper.Map<UpdateBranchResult>(updated);
            return result;
        }
    }
}
