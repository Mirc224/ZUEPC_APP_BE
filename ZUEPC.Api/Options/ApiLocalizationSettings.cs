using Microsoft.AspNetCore.Localization;

namespace ZUEPC.Options;

public static class ApiLocalizationSettings
{
    public static RequestLocalizationOptions GetLocalizationOptions(WebApplicationBuilder builder)
    {
        var supportedCultures = builder.Configuration.GetSection("Cultures:SupportedCultures").Get<string[]>();
        var defaultCulture = builder.Configuration.GetSection("Cultures:Default").Value;
        
        builder.Services.AddLocalization();

        var localizationOptions = new RequestLocalizationOptions()
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        localizationOptions.DefaultRequestCulture = new RequestCulture(defaultCulture);
        localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

        return localizationOptions;
    }
}
