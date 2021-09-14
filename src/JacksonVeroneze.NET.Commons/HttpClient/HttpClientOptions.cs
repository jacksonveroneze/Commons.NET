using System;

namespace JacksonVeroneze.NET.Commons.HttpClient
{
    public class HttpClientOptions
    {
        public Uri BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; }
    }
}
