using Microsoft.AspNetCore.Localization;

namespace ZUEPC.Options;

public static class ApiLocalizationSettings
{
	public static RequestLocalizationOptions GetLocalizationOptions(WebApplicationBuilder builder)
	{
		string[] supportedCultures = builder.Configuration.GetSection("Cultures:SupportedCultures").Get<string[]>();
		string defaultCulture = builder.Configuration.GetSection("Cultures:Default").Value;

		builder.Services.AddLocalization();

		RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
			.AddSupportedCultures(supportedCultures)
			.AddSupportedUICultures(supportedCultures);
		localizationOptions.DefaultRequestCulture = new RequestCulture(defaultCulture);
		localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

		return localizationOptions;
	}
}
