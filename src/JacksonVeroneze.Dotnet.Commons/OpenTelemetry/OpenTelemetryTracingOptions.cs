namespace JacksonVeroneze.Dotnet.Commons.OpenTelemetry
{
    public class OpenTelemetryTracingOptions
    {
        public string ApplicationName { get; set; }

        public string JaegerAgentHost { get; set; }

        public int JaegerAgentPort { get; set; }
    }
}