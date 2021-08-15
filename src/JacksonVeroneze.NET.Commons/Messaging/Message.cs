using System;

namespace JacksonVeroneze.NET.Commons.Messaging
{
    public abstract class Message
    {
        public string MessageType { get; private set; }

        public Guid AggregateId { get; protected set; }

        protected Message()
            => MessageType = GetType().Name;
    }
}
