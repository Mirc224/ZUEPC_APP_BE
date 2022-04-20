using ZUEPC.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRequestLocalization(ApiLocalizationSettings.GetLocalizationOptions(builder));

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
