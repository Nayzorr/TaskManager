using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sentry;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using TaskManager.Api;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Module = TaskManager.Api.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = EnvironmentVariables.JwtIssuer,
        ValidAudience = EnvironmentVariables.JwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.JwtKey))
    };
});

builder.Services.AddSentry();
builder.WebHost.UseSentry(o =>
{
    o.Dsn = "https://2f886863ab564c028709c0104abc6143@o4504724543569920.ingest.sentry.io/4504724554645504";
    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
    // We recommend adjusting this value in production.
    o.TracesSampleRate = 1.0;
    o.SampleRate = 1;
    o.DisableDiagnosticSourceIntegration();
    o.DiagnosticLevel = SentryLevel.Debug;
    o.Debug = true;
    o.BeforeSend = sentryEvent =>
    {
        if (sentryEvent.Exception != null
          && sentryEvent.Exception.Message.Contains("Some useless exception :)"))
        {
            return null; // Don't send this event to Sentry
        }

        sentryEvent.ServerName = null; // Never send Server Name to Sentry
        return sentryEvent;
    };

    o.AddExceptionFilterForType<OperationCanceledException>();

});

SentrySdk.CaptureMessage("Hello Sentry");

builder.Services.AddMvc();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new Module()));

builder.Services.AddHttpClient();
builder.Services
  .AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = EnvironmentVariables.SwaggerEndpointTitle, Version = "v1" });
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
          Description = "Copy 'Bearer ' + valid JWT token into field",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
      });
      c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
        });
      c.AddEnumsWithValuesFixFilters();
  });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(EnvironmentVariables.Cors)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

var basePath = Environment.GetEnvironmentVariable("SERVICE_BASE_PATH")?.Insert(0, "/") ?? "/taskmanagerapi";

app.UsePathBase(basePath);

app.UseRouting();

app.UseSentryTracing();

app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"{basePath}{EnvironmentVariables.SwaggerEndpointUrl}", EnvironmentVariables.SwaggerEndpointTitle);
});

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.Run();
