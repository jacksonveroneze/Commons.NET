namespace JacksonVeroneze.NET.Commons.OpenTelemetry
{
    public class OpenTelemetryTracingOptions
    {
        public string ApplicationName { get; set; }

        public string JaegerAgentHost { get; set; }

        public int JaegerAgentPort { get; set; }
    }
}