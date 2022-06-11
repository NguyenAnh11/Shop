using MediatR;

namespace Shop.SharedKernel.Events
{
    public class EntityUpdated<T> : INotification where T : BaseEntity
    {
        public T Model { get; private set; }
        public EntityUpdated(T model)
        {
            Model = model;
        }
    }
}
