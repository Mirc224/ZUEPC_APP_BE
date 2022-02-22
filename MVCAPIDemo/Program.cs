using ZUEPC.Options;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRequestLocalization(ApiLocalizationSettings.GetLocalizationOptions(builder));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
