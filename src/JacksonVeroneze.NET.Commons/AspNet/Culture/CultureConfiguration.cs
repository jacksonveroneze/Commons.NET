using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace JacksonVeroneze.NET.Commons.AspNet.Culture
{
    public static class CultureConfiguration
    {
        private const string Culture = "pt-BR";
        public static IApplicationBuilder UseCultureConfiguration(this IApplicationBuilder app)
            => app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(Culture, Culture),
                SupportedCultures = {new CultureInfo(Culture)},
                SupportedUICultures = {new CultureInfo(Culture)}
            });
    }
}