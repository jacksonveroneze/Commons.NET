using JacksonVeroneze.NET.Commons.Notifications;

namespace JacksonVeroneze.NET.Commons.ValidationPattern
{
    public interface IValidationRule<T>
    {
        Notification Error { get; }

        bool Validate(T value);

        bool StopValidation { get; set; }
    }
}
