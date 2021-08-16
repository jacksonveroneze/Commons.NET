using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.Messaging;
using JacksonVeroneze.NET.Commons.Messaging.CommonMessaging.DomainEvents;

namespace JacksonVeroneze.NET.Commons.Bus
{
    public interface IBus
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;
    }
}
