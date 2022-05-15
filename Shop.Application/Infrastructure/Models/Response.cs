namespace Shop.Application.Infrastructure.Models
{
    public class Response
    {
        public bool Success { get; init; }
        public List<string> Messages { get; init; } = new();

        public Response(bool success)
        {
            Success = success;
        }

        public Response(bool success, string message) : this(success)
        {
            Messages.Add(message);
        }

        public Response(bool success, IList<string> messages) : this(success)
        {
            if(messages != null)
                Messages.AddRange(messages);
        }

        public static Response Ok() => new(true);

        public static Response Bad() => new(false);

        public static Response Bad(string message) => new(false, message);

        public static Response Bad(IList<string> messages) => new(false, messages);
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

        public Response(bool success, IList<string> messages) : base(success, messages)
        {
        }

        public new static Response<T> Ok() => new(true);

        public static Response<T> Ok(T data) => new(data, true);

        public new static Response<T> Bad() => new(false);

        public new static Response<T> Bad(string message) => new(false, message);

        public new static Response<T> Bad(IList<string> messages) => new(false, messages);
    }
}
