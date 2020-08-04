using System;
using AutoMapper;
using Commander.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json.Serialization;

namespace Commander
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      // Database context
      services.AddDbContext<CommanderContext>(opt =>
        opt.UseSqlServer(Configuration.GetConnectionString("CommanderConnection"))
      );

      // Endpoints
      services.AddControllers().AddNewtonsoftJson(action =>
      {
        action.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      });

      // Auto mapping
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      // API versioning
      services.AddApiVersioning(config =>
      {
        config.DefaultApiVersion = new ApiVersion(1, 0);
        config.AssumeDefaultVersionWhenUnspecified = true;
        config.ReportApiVersions = true;
      });
      services.AddVersionedApiExplorer();

      // IOC
      services.AddScoped<ICommanderRepo, SqlCommanderRepo>();

      // Swagger configuration
      services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
      services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionProvider)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseApiVersioning();

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        foreach (var apiVersion in apiVersionProvider.ApiVersionDescriptions)
        {
          c.SwaggerEndpoint(
            $"/swagger/{apiVersion.GroupName}/swagger.json",
            $"Commander API v{apiVersion.GroupName.ToUpperInvariant()}"
          );
        }
      });

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }


}
