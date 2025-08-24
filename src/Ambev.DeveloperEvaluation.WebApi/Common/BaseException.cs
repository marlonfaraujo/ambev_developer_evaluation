namespace Ambev.DeveloperEvaluation.WebApi.Common
{
    public class BaseException : Exception
    {
        public int Code { get; set; }

        public BaseException(string message, int code = 0) : base(message)
        {
            this.Code = code;
        }

        public BaseException(string message, Exception e, int code = 0) : base(message, e)
        {
            this.Code = code;
        }
    }
}
