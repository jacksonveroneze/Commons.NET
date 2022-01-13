using System;

namespace JacksonVeroneze.NET.Commons.DomainObjects
{
    public abstract class Entity : EntityId
    {
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; private set; }

        public DateTime? DeletedAt { get; private set; }

        public int Version { get; private set; } = 1;

        public Guid TenantId { get; private set; }

        public Entity ShallowCopy()
            => (Entity)MemberwiseClone();

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.Now;
            Version++;
        }

        public void MarkAsDeleted()
            => DeletedAt = DateTime.Now;

        public override string ToString()
            => $"{GetType().Name}: Id: {Id}, CreatedAt: {CreatedAt}, " +
               $"UpdatedAt: {UpdatedAt}, DeletedAt: {DeletedAt}, Version: {Version}, TenantId: {TenantId}";
    }
}