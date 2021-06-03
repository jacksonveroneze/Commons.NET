namespace JacksonVeroneze.NET.Commons.OpenTelemetry
{
    public class OpenTelemetryTracingOptions
    {
        public string ApplicationName { get; set; }
        
        public bool ShowConsoleExporter { get; set; }
        
        public bool UseJaeger { get; set; }

        public string JaegerAgentHost { get; set; }

        public int JaegerAgentPort { get; set; }
    }
}