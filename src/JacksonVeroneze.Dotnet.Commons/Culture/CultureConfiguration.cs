using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace JacksonVeroneze.Dotnet.Commons.Culture
{
    public static class CultureConfiguration
    {
        public static IApplicationBuilder UseCultureConfiguration(this IApplicationBuilder app)
            => app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR"),
                SupportedCultures = {new CultureInfo("pt-BR")},
                SupportedUICultures = {new CultureInfo("pt-BR")}
            });
    }
}