namespace Ambev.DeveloperEvaluation.Application.Exceptions
{
    public class CartFinishedException : ApplicationException
    {

        public CartFinishedException(string? message) : base(message)
        {
        }

        public CartFinishedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}