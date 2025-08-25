using Ambev.DeveloperEvaluation.Application.Requests;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Adapters
{
    public class MediatRRequestAdapter<TRequest, TResult> : IRequest<TResult> where TRequest : IRequestApplication<TResult>
    {
        public TRequest Request { get; set; }

        public MediatRRequestAdapter(TRequest request)
        {
            Request = request;
        }
    }
}
