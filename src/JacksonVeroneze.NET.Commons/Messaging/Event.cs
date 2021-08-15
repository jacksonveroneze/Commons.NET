using System;
using MediatR;

namespace JacksonVeroneze.NET.Commons.Messaging
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; } = DateTime.Now;

        protected Event(Guid aggregateId)
            => AggregateId = aggregateId;
    }
}
