namespace JacksonVeroneze.NET.Commons.Database.Configuration.Document
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public bool EnableSensitiveDataLogging { get; set; }
    }
}