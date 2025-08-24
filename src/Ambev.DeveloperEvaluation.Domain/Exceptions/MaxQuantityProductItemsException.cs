
namespace Ambev.DeveloperEvaluation.Domain.Exceptions
{
    public class MaxQuantityProductItemsException : DomainException
    {
        public MaxQuantityProductItemsException(string message) : base(message)
        {
        }

        public MaxQuantityProductItemsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
