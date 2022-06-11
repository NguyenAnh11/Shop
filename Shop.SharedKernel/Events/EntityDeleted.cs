using MediatR;

namespace Shop.SharedKernel.Events
{
    public class EntityDeleted<T> : INotification where T: BaseEntity
    {
        public T Model { get; private set; }
        public EntityDeleted(T model)
        {
            Model = model;
        }
    }
}
