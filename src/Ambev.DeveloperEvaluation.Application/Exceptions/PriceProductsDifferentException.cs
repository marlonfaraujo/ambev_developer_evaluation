namespace Ambev.DeveloperEvaluation.Application.Exceptions
{
    public class PriceProductsDifferentException : ApplicationException
    {
        public PriceProductsDifferentException(string message) : base(message)
        {
        }

        public PriceProductsDifferentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
