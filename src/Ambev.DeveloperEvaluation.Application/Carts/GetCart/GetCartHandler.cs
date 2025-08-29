using Ambev.DeveloperEvaluation.Application.Requests;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart
{
    public class GetCartHandler : IRequestApplicationHandler<GetCartQuery, GetCartResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            // Adicione validação se necessário
            var cart = await _cartRepository.GetByIdAsync(query.Id, cancellationToken);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID {query.Id} not found");
            return _mapper.Map<GetCartResult>(cart);
        }
    }
}
