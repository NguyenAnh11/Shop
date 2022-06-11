using Shop.SharedKernel.Events; 

namespace Shop.Application.Infrastructure.Extensions
{
    public static class EventPublisherExtension
    {
        public static Task EntityInserted<T>(this IMediator mediator, T model)
            where T: BaseEntity
            => mediator.Publish(new EntityInserted<T>(model));

        public static Task EntityUpdated<T>(this IMediator mediator, T model)
            where T : BaseEntity
            => mediator.Publish(new EntityUpdated<T>(model));

        public static Task EntityDeleted<T>(this IMediator mediator, T model)
            where T : BaseEntity
            => mediator.Publish(new EntityDeleted<T>(model));
    }
}
