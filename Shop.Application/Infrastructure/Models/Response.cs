namespace Shop.Application.Infrastructure.Models
{
    public class Response
    {
        public bool Success { get; init; }
        public string Message { get; init; }

        public Response(bool success)
        {
            Success = success;
        }

        public Response(bool success, string message) : this(success)
        {
            Message = message;
        }

        public static Response Ok() => new(true);

        public static Response Bad() => new(false);

        public static Response Bad(string message) => new(false, message);
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }

        public Response(bool success) : base(success)
        {
        }

        public Response(T data, bool success) : base(success)
        {
            Data = data;
        }

        public Response(bool success, string message) : base(success, message)
        {
        }

        public new static Response<T> Ok() => new(true);

        public static Response<T> Ok(T data) => new(data, true);

        public new static Response<T> Bad() => new(false);

        public new static Response<T> Bad(string message) => new(false, message);
    }
}
