using System;

namespace JacksonVeroneze.NET.Commons.HttpClient
{
    public class CircuitBreakerPolicyOptions
    {
        public TimeSpan DurationOfBreak { get; set; } = TimeSpan.FromSeconds(30);
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 12;
    }
}
