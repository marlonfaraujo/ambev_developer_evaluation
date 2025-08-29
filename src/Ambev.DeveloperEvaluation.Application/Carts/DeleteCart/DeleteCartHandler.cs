using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart
{
    public class DeleteCartHandler : IRequestApplicationHandler<DeleteCartCommand, DeleteCartResult>
    {
        private readonly ICartRepository _cartRepository;

        public DeleteCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<DeleteCartResult> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
        {
            // Adicione validação se necessário
            var success = await _cartRepository.DeleteAsync(command.Id, cancellationToken);
            return new DeleteCartResult { Success = success };
        }
    }
}
