namespace Ambev.DeveloperEvaluation.Application.Requests
{
    public interface IRequestApplicationHandler<TRequest, TResult> where TRequest : IRequestApplication<TResult>
    {
        Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
