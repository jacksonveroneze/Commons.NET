using System;

namespace JacksonVeroneze.NET.Commons.DomainObjects
{
    public abstract class Entity : EntityId
    {
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

        public DateTimeOffset? UpdatedAt { get; private set; }

        public DateTimeOffset? DeletedAt { get; private set; }

        public int Version { get; private set; } = 1;

        public Guid TenantId { get; private set; }

        public Entity ShallowCopy()
            => (Entity)MemberwiseClone();

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTimeOffset.Now;
            Version++;
        }

        public void MarkAsDeleted()
            => DeletedAt = DateTimeOffset.Now;

        public override string ToString()
            => $"{GetType().Name}: Id: {Id}, CreatedAt: {CreatedAt}, " +
               $"UpdatedAt: {UpdatedAt}, DeletedAt: {DeletedAt}, Version: {Version}, TenantId: {TenantId}";
    }
}