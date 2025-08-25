using Ambev.DeveloperEvaluation.Application.Requests;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Adapters
{
    public class MediatRRequestHandlerAdapter<TRequest, TResult> : IRequestHandler<MediatRRequestAdapter<TRequest, TResult>, TResult> where TRequest : IRequestApplication<TResult>
    {
        private readonly IRequestApplicationHandler<TRequest, TResult> _handler;

        public MediatRRequestHandlerAdapter(IRequestApplicationHandler<TRequest, TResult> handler)
        {
            _handler = handler;
        }

        public Task<TResult> Handle(MediatRRequestAdapter<TRequest,TResult> request, CancellationToken cancellationToken)
        {
            return _handler.Handle(request.Request, cancellationToken);
        }
    }
}
