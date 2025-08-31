using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch
{
    public class DeleteBranchHandler : IRequestApplicationHandler<DeleteBranchCommand, DeleteBranchResult>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IQueryDatabaseService _queryDbService;
        private readonly ICartRepository _cartRepository;

        public DeleteBranchHandler(IBranchRepository branchRepository, IQueryDatabaseService queryDbService, ICartRepository cartRepository)
        {
            _branchRepository = branchRepository;
            _queryDbService = queryDbService;
            _cartRepository = cartRepository;
        }

        public async Task<DeleteBranchResult> Handle(DeleteBranchCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteBranchCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await validateExistingBranchById();
            await validateBranchIdInSale();
            await validateBranchIdInCart();

            var success = await _branchRepository.DeleteAsync(command.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Branch with ID {command.Id} not found");

            return new DeleteBranchResult { Success = true };

            async Task validateExistingBranchById(){
                var existing = await _branchRepository.GetByIdAsync(command.Id, cancellationToken);
                if (existing == null)
                    throw new KeyNotFoundException($"Branch with ID {command.Id} not found");
            }
            async Task validateBranchIdInSale()
            {
                var hasBranchIdInSale = await _queryDbService.BranchInSales(command.Id);
                if (hasBranchIdInSale)
                    throw new InvalidOperationException($"Branch with ID {command.Id} cannot be deleted because it is referenced in sales");
            }

            async Task validateBranchIdInCart()
            {
                var filters = new Dictionary<string, object>();
                var options = new QueryOptions();
                filters.Add("BranchSaleId", command.Id);
                options.Filters = filters;

                var carts = await _cartRepository.GetPagedAsync(options, cancellationToken);
                if (carts != null && carts.Items.Any())
                {
                    throw new InvalidOperationException($"Branch with ID {command.Id} cannot be deleted because it is referenced in carts");
                }
            }
        }
    }
}
