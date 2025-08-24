using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetBranchHandler : IRequestHandler<GetBranchQuery, GetBranchResult>
    {

        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public GetBranchHandler(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<GetBranchResult> Handle(GetBranchQuery query, CancellationToken cancellationToken)
        {
            var validator = new GetBranchQueryValidator();
            var validationResult = await validator.ValidateAsync(query, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var branch = await _branchRepository.GetByIdAsync(query.Id, cancellationToken);
            if (branch == null)
                throw new KeyNotFoundException($"Record with ID {query.Id} not found");

            return _mapper.Map<GetBranchResult>(branch);
        }
    }
}
