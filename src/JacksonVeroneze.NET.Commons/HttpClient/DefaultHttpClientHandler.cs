using System.Net;
using System.Net.Http;
using System.Security.Authentication;

namespace JacksonVeroneze.NET.Commons.HttpClient
{
    public class DefaultHttpClientHandler : HttpClientHandler
    {
        public DefaultHttpClientHandler()
        {
            AutomaticDecompression =
                DecompressionMethods.Brotli |
                DecompressionMethods.Deflate |
                DecompressionMethods.GZip;

            AllowAutoRedirect = true;
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
        }
    }
}
