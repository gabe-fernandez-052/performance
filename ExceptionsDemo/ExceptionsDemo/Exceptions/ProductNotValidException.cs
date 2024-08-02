namespace ExceptionsDemo.Exceptions
{
    public class ProductNotValidException : Exception
    {
        public ProductNotValidException(string message) : base(message)
        {
        }

        public ProductNotValidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ProductNotValidException()
        {
        }
    }
}