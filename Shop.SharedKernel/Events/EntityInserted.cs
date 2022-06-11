using MediatR;

namespace Shop.SharedKernel.Events
{
    public class EntityInserted<T> : INotification where T: BaseEntity
    {
        public T Model { get; private set; }
        public EntityInserted(T model)
        {
            Model = model;
        }
    }
}
