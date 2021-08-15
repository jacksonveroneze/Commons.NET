using System;
using MediatR;

namespace JacksonVeroneze.NET.Commons.Messaging.CommonMessaging.DomainEvents
{
    public abstract class DomainEvent : Event, IRequest
    {
        protected DomainEvent(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
