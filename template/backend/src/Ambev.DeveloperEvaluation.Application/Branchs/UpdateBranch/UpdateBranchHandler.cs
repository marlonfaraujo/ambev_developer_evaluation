using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch
{
    public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchResult>
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
            if (existing != null)
                throw new InvalidOperationException($"Record already exists");

            var branch = _mapper.Map<Branch>(command);

            var created = await _branchRepository.CreateAsync(branch, cancellationToken);
            var result = _mapper.Map<UpdateBranchResult>(created);
            return result;
        }
    }
}
