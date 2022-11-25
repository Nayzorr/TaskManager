using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManager.Api;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Module = TaskManager.Api.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

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

builder.Services.AddMvc();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new Module()));

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

var app = builder.Build();

var basePath = Environment.GetEnvironmentVariable("SERVICE_BASE_PATH")?.Insert(0, "/") ?? "/taskmanagerapi";

app.UsePathBase(basePath);

app.UseRouting();

app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"{basePath}{EnvironmentVariables.SwaggerEndpointUrl}", EnvironmentVariables.SwaggerEndpointTitle);
});

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.Run();
