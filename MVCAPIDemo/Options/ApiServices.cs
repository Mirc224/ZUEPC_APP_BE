using DataAccess.Data.User;
using DataAccess.DbAccess;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using ZUEPC.Application.Filters;
using ZUEPC.Application.Import.Services;
using ZUEPC.Auth.Services;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.Localization;
using ZUEPC.DataAccess.Data.Persons;

namespace ZUEPC.Options;

public static class ApiServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
		// User
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<IUserData, UserData>();
		// Publication
        builder.Services.AddSingleton<IPublicationData, PublicationInMemoryData>();
        builder.Services.AddSingleton<IPublicationNameData, PublicationNameInMemoryData>();
        builder.Services.AddSingleton<IPublicationExternDatabaseIdData, PublicationExternDatabaseIdInMemoryData>();
        builder.Services.AddSingleton<IPublicationIdentifierData, PublicationIdentifierInMemoryData>();
		// Person
        builder.Services.AddSingleton<IPersonData, PersonInMemoryData>();
        builder.Services.AddSingleton<IPersonNameData, PersonNameInMemoryData>();
        builder.Services.AddSingleton<IPersonExternDatabaseIdData, PersonExternDatabaseIdInMemoryData>();
		// Institution
		builder.Services.AddSingleton<IInstitutionData, InstitutionInMemoryData>();
		builder.Services.AddSingleton<IInstitutionExternDatabaseIdData, InstitutionExternDatabaseIdInMemoryData>();
		builder.Services.AddSingleton<IInstitutionNameData, InstitutionNameInMemoryData>();
		// Publication activity
		builder.Services.AddSingleton<IPublicationActivityData, PublicationActivityInMemoryData>();
		// Publication author
		builder.Services.AddSingleton<IPublicationAuthorData, PublicationAuthorInMemoryData>();
		// Related publication
		builder.Services.AddSingleton<IRelatedPublicationData, RelatedPublicationInMemoryData>();

		builder.Services.AddMediatR(typeof(Program));
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddTransient<DataAnnotations>();
		ConfigureAuthentication(builder);

        builder.Services
            .AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation(
            fv => fv.RegisterValidatorsFromAssemblyContaining<Program>())
			.AddNewtonsoftJson(); ;


		builder.Services.AddLocalization();
		builder.Services
			.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo() { Title = "UNIZA KIS", Version = "v1" });


            x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer schceme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            x.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }

    public static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
		builder.Services.AddSingleton<AuthenticationService>();
		builder.Services.AddSingleton<ImportService>();
		var jwtSettings = new JwtSettings();

        builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
		builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(jwtSettings)));
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = key,
			ValidateIssuer = false,
			ValidateAudience = false,
			RequireExpirationTime = false,
			ValidateLifetime = true
		};
		builder.Services.AddSingleton(tokenValidationParameters);
		builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
           {
               x.SaveToken = true;
               x.TokenValidationParameters = tokenValidationParameters;
           });

    }
}
