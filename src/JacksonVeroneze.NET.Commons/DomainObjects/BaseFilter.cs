using System;
using System.Linq.Expressions;

namespace JacksonVeroneze.NET.Commons.DomainObjects
{
    public abstract class BaseFilter<T> where T : EntityRoot
    {
        public abstract Expression<Func<T, bool>> ToExpression();
    }
}
