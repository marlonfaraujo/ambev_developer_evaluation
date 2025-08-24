using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetBranchHandler : IRequestHandler<GetBranchCommand, GetBranchResult>
    {

        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public GetBranchHandler(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<GetBranchResult> Handle(GetBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new GetBranchCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var branch = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
            if (branch == null)
                throw new KeyNotFoundException($"Record with ID {command.Id} not found");

            return _mapper.Map<GetBranchResult>(branch);
        }
    }
}
