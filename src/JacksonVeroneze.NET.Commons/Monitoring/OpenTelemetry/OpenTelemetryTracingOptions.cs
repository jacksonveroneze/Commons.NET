namespace JacksonVeroneze.NET.Commons.Monitoring.OpenTelemetry
{
    public class OpenTelemetryTracingOptions
    {
        public string ApplicationName { get; set; }
        
        public bool ShowConsoleExporter { get; set; }
        
        public bool UseJaeger { get; set; }

        public string JaegerAgentHost { get; set; }

        public int JaegerAgentPort { get; set; }
        
        public bool UseGrafanaAgent { get; set; }
        
        public string GrafanaAgentHost { get; set; }
    }
}