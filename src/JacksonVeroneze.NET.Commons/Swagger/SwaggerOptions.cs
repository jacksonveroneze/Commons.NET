namespace JacksonVeroneze.NET.Commons.Swagger
{
    public class SwaggerOptions
    {
        public string Title { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }
        
        public bool IncludeXmlComments { get; set; }
        
        public string XmlCommentsPath { get; set; }
    }
}