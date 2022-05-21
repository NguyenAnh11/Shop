using Shop.Application.Infrastructure.Exceptions;

namespace Shop.Infrastructure.Mvc.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if(exception is NotFoundException)
            {
                context.Result = new NotFoundResult();

                context.ExceptionHandled = true;
            }
        }
    }
}
