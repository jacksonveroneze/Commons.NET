using JacksonVeroneze.NET.Commons.DomainObjects;

namespace JacksonVeroneze.NET.Commons.Exceptions
{
    public static class ExceptionsFactory
    {
        public static NotFoundException FactoryNotFoundException<TEntity, TId>(TId id)
            where TEntity : Entity where TId : EntityId
            => new($"{ErrorMessages.ItemNotFound} ({id}) em '{nameof(TEntity)}'");

        public static DomainException FactoryDomainException(string message)
            => new(message);
    }
}
