using Asp.Versioning;

namespace CIS.Infrastructure.StartupExtensions;


public static class CisApiVersioning
{
    public static WebApplicationBuilder AddCisApiVersioning(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(
          options =>
          {
              options.ReportApiVersions = true;
              options.AssumeDefaultVersionWhenUnspecified = true;
              options.ApiVersionReader = new HeaderApiVersionReader("api-version");

          }).AddApiExplorer(options =>
                    {
                        // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
                        // note: the specified format code will format the version as "'v'major"
                        options.GroupNameFormat = "'v'V";

                        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                        // can also be used to control the format of the API version in route templates
                        options.SubstituteApiVersionInUrl = true;
                    });

        return builder;
    }
}
