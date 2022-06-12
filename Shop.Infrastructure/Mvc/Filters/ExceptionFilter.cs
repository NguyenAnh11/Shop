using Shop.Application.Infrastructure.Exceptions;

namespace Shop.Infrastructure.Mvc.Filters
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if(exception is NotFoundException)
            {
                context.Result = new BadRequestResult();

                context.ExceptionHandled = true;
            }
        }
    }
}
