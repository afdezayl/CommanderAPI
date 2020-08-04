using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Commander
{
  public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
  {
    readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
      this.provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
      foreach (var description in provider.ApiVersionDescriptions)
      {
        options.SwaggerDoc(
          description.GroupName,
            new OpenApiInfo()
            {
              Title = $"Commander API v{description.ApiVersion}",
              Version = description.ApiVersion.ToString(),
            });
        options.OperationFilter<RemoveVersionFromParameter>();
        options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
      }
    }

    internal class RemoveVersionFromParameter : IOperationFilter
    {
      public void Apply(OpenApiOperation operation, OperationFilterContext context)
      {
        var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
        operation.Parameters.Remove(versionParameter);
      }
    }

    internal class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
      public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
      {
        var paths = new OpenApiPaths();

        foreach (var path in swaggerDoc.Paths)
        {
          paths.Add(path.Key.Replace("v{version}", $"v{swaggerDoc.Info.Version}"), path.Value);
        }
        swaggerDoc.Paths = paths;
      }
    }
  }
}