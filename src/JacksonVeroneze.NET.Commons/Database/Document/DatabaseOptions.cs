using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.NET.Commons.Database.Document
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
        
        public ILogger Logger { get; set; }
    }
}