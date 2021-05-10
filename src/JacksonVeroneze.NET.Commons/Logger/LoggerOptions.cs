namespace JacksonVeroneze.NET.Commons.Logger
{
    public class LoggerOptions
    {
        public string ApplicationName { get; set; }

        public string Environment { get; set; }

        public string CurrentDirectory { get; set; }

        public bool EnableApplicationInsights { get; set; } = false;
    }
}