using System;

namespace JacksonVeroneze.NET.Commons.DomainObjects
{
    public class EntityId : SimpleId<Guid>
    {
        protected EntityId()
            => Id = Guid.NewGuid();

        public EntityId(Guid id)
            => Id = id;

        public override string ToString() => Id.ToString();
    }
}
