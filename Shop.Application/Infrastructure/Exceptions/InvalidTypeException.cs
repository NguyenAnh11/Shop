namespace Shop.Application.Infrastructure.Exceptions
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException()
        {

        }

        public InvalidTypeException(string message) : base(message)
        {

        }
    }
}
