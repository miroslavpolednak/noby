using System.Globalization;
using System.Reflection;
using System.Text;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NOBY.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace NOBY.Api.StartupExtensions;

internal static class NobySwagger
{
    static string xmlFileName(Type type) => type.GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");

    public static WebApplicationBuilder AddNobySwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        // konfigurace pro generátor JSON souboru
        builder.Services.AddSwaggerGen(x =>
        {
            // add a custom operation filter which sets default values
            x.OperationFilter<SwaggerDefaultValues>();
            x.OperationFilter<AddAllHttpStatusCodes>();

            // zapojení rozšířených anotací nad controllery
            x.EnableAnnotations();
            //x.UseOneOfForPolymorphism();

            x.SupportNonNullableReferenceTypes();
            //x.UseAllOfToExtendReferenceSchemas();

            // všechny parametry budou camel case
            x.DescribeAllParametersInCamelCase();
            x.UseInlineDefinitionsForEnums();

            x.CustomSchemaIds(type =>
            {
                string s = type.ToString();
                var idx = s.LastIndexOf('.');
                return idx > 0 ? s[(idx + 1)..] : type.ToString().Replace('+', '_');
            });
            x.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");

            // generate the XML docs that'll drive the swagger docs
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName(typeof(Program))));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "NOBY.ApiContracts.xml"));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ExternalServices.AddressWhisperer.xml"));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DomainServices.CodebookService.Contracts.xml"));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SharedTypes.xml"));

            x.SchemaFilter<NewLineReplacementFilter>();
            x.SchemaFilter<SwaggerOneOfSchemaFilter>();
            x.SchemaFilter<EnumValuesDescriptionSchemaFilter>();

            x.OperationFilter<NewLineReplacementFilter>();
            x.OperationFilter<RollbackRequestSupportFilter>();
            x.OperationFilter<ApplySwaggerNobyAttributes>();
            x.OperationFilter<Endpoints.RealEstateValuation.ApplySwaggerRealEstateValuationAttributes>();
        });

        return builder;
    }

    private sealed class AddAllHttpStatusCodes : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses == null)
            {
                operation.Responses = new OpenApiResponses();
            }

            // Add other status codes as needed
            if (!operation.Responses.ContainsKey(StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture)))
            {
                operation.Responses[StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture)] = new OpenApiResponse
                {
                    Description = "Bad Request"
                };
            }

            if (!operation.Responses.ContainsKey(StatusCodes.Status404NotFound.ToString(CultureInfo.InvariantCulture)))
            {
                operation.Responses[StatusCodes.Status404NotFound.ToString(CultureInfo.InvariantCulture)] = new OpenApiResponse
                {
                    Description = "Not Found"
                };
            }

            if (!operation.Responses.ContainsKey(StatusCodes.Status500InternalServerError.ToString(CultureInfo.InvariantCulture)))
            {
                operation.Responses[StatusCodes.Status500InternalServerError.ToString(CultureInfo.InvariantCulture)] = new OpenApiResponse
                {
                    Description = "Internal Server Error"
                };
            }

            if (!operation.Responses.ContainsKey(StatusCodes.Status401Unauthorized.ToString(CultureInfo.InvariantCulture)))
            {
                operation.Responses[StatusCodes.Status401Unauthorized.ToString(CultureInfo.InvariantCulture)] = new OpenApiResponse
                {
                    Description = "Unauthorized"
                };
            }

            if (!operation.Responses.ContainsKey(StatusCodes.Status403Forbidden.ToString(CultureInfo.InvariantCulture)))
            {
                operation.Responses[StatusCodes.Status403Forbidden.ToString(CultureInfo.InvariantCulture)] = new OpenApiResponse
                {
                    Description = "Forbidden"
                };
            }
        }
    }

    private sealed class EnumValuesDescriptionSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var requestedType = Nullable.GetUnderlyingType(context.Type) ?? context.Type;

            if (!requestedType.IsEnum)
                return;

            var sb = new StringBuilder(schema.Description ?? string.Empty);

            if (sb.Length > 0)
                sb.Append("\n\n");

            sb.Append("<small>Enum Values</small>");

            sb.Append("<ul>");

            foreach (var enumValue in Enum.GetValues(requestedType))
            {
                sb.Append(CultureInfo.InvariantCulture, $"<li>{Convert.ToInt32(enumValue, CultureInfo.InvariantCulture)} - {enumValue}</li>");
            }

            sb.Append("</ul>");

            schema.Description = sb.ToString();
        }
    }

    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    private sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this._provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc(Constants.OpenApiDocName, CreateInfoForApiVersionAll());

            options.DocInclusionPredicate((openApiDocName, apiDesc) =>
            {
                // Here we can decide if api endpoint should be in specific open api json file. (Currently we have only one file and all api versions gone be in one file)
                return true;
            });
        }

        private static OpenApiInfo CreateInfoForApiVersionAll()
        {
            var text = new StringBuilder("Obecná specifikace <b>error handlingu</b> <ul><li>[https://wiki.kb.cz/pages/viewpage.action?pageId=589534698](https://wiki.kb.cz/pages/viewpage.action?pageId=589534698)</li></ul>Specifikace <b>HTTP hlaviček</b> <ul><li>[https://wiki.kb.cz/pages/viewpage.action?pageId=513345095](https://wiki.kb.cz/pages/viewpage.action?pageId=513345095)</li></ul>");

            var info = new OpenApiInfo()
            {
                Title = "NOBY FRONTEND API",
                Version = "All version"
            };

            info.Description = text.ToString();

            return info;
        }
    }

    /// <summary>
    /// Represents the OpenAPI/Swashbuckle operation filter used to document information provided, but not used.
    /// </summary>
    /// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
    /// Once they are fixed and published, this class can be removed.</remarks>
    private sealed class SwaggerDefaultValues : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1752#issue-663991077
            foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
            {
                // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/b7cf75e7905050305b115dd96640ddd6e74c7ac9/src/Swashbuckle.AspNetCore.SwaggerGen/SwaggerGenerator/SwaggerGenerator.cs#L383-L387
                var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString(CultureInfo.InvariantCulture);
                var response = operation.Responses[responseKey];

                foreach (var contentType in response.Content.Keys)
                {
                    if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }

            if (operation.Parameters == null)
            {
                return;
            }

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                parameter.Description ??= description.ModelMetadata?.Description;

                if (parameter.Schema.Default == null &&
                     description.DefaultValue != null &&
                     description.DefaultValue is not DBNull &&
                     description.ModelMetadata is ModelMetadata modelMetadata)
                {
                    // REF: https://github.com/Microsoft/aspnet-api-versioning/issues/429#issuecomment-605402330
                    var json = JsonSerializer.Serialize(description.DefaultValue, modelMetadata.ModelType);
                    parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}