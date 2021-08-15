using System.Collections.Generic;
using JacksonVeroneze.NET.Commons.Notifications;

namespace JacksonVeroneze.NET.Commons.ValidationPattern
{
    public interface IValidator<TModel>
    {
        IValidator<TModel> AddRule(IValidationRule<TModel> rule);

        IList<Notification> Validate(TModel model);
    }
}
