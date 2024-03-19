using System.Globalization;
using System.Reflection;
using System.Text;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NOBY.Api.Endpoints.Codebooks.CodebookMap;
using NOBY.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace NOBY.Api.StartupExtensions;

internal static class NobySwagger
{
    static string xmlFileName(Type type) => type.GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");

    public static WebApplicationBuilder AddNobySwagger(this WebApplicationBuilder builder, ICodebookMap codebookMap)
    {
        builder.Services.AddEndpointsApiExplorer();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        // konfigurace pro generátor JSON souboru
        builder.Services.AddSwaggerGen(x =>
        {
            // add a custom operation filter which sets default values
            x.OperationFilter<SwaggerDefaultValues>();

            // zapojení rozšířených anotací nad controllery
            x.EnableAnnotations();
            //x.UseOneOfForPolymorphism();

            x.SupportNonNullableReferenceTypes();
            x.UseAllOfToExtendReferenceSchemas();

            // všechny parametry budou camel case
            x.DescribeAllParametersInCamelCase();
            x.UseInlineDefinitionsForEnums();

            x.CustomSchemaIds(type => type.ToString().Replace('+', '_'));
            x.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");

            // generate the XML docs that'll drive the swagger docs
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName(typeof(Program))));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ExternalServices.AddressWhisperer.xml"));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DomainServices.CodebookService.Contracts.xml"));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "NOBY.Dto.xml"));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SharedTypes.xml"));

            x.SchemaFilter<NewLineReplacementFilter>();
            x.SchemaFilter<SwaggerOneOfSchemaFilter>();
            x.SchemaFilter<CodebookGetAllSchemaFilter>(codebookMap);
            x.SchemaFilter<EnumValuesDescriptionSchemaFilter>();

            x.OperationFilter<NewLineReplacementFilter>();
            x.OperationFilter<RollbackRequestSupportFilter>();
            x.OperationFilter<ApplySwaggerNobyAttributes>();
            x.OperationFilter<Endpoints.RealEstateValuation.ApplySwaggerRealEstateValuationAttributes>();
        });

        return builder;
    }

    private sealed class CodebookGetAllSchemaFilter : ISchemaFilter
    {
        private readonly List<Type> _getAllResponsePossibleTypes;

        public CodebookGetAllSchemaFilter(CodebookMap codebookMap)
        {
            _getAllResponsePossibleTypes = codebookMap.GroupBy(c => c.ReturnType, c => c.ReturnType).Select(c => c.First()).ToList();
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(Endpoints.Codebooks.GetAll.GetAllResponseItem))
                return;

            var codebookCollectionProperty = schema.Properties[nameof(Endpoints.Codebooks.GetAll.GetAllResponseItem.Codebook).ToLowerInvariant()];

            foreach (var type in _getAllResponsePossibleTypes)
            {
                codebookCollectionProperty.Items.OneOf.Add(context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository));
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
        private readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var text = new StringBuilder("Obecná specifikace <b>error handlingu</b> <ul><li>[https://wiki.kb.cz/pages/viewpage.action?pageId=589534698](https://wiki.kb.cz/pages/viewpage.action?pageId=589534698)</li></ul>Specifikace <b>HTTP hlaviček</b> <ul><li>[https://wiki.kb.cz/pages/viewpage.action?pageId=513345095](https://wiki.kb.cz/pages/viewpage.action?pageId=513345095)</li></ul>");

            var info = new OpenApiInfo()
            {
                Title = "NOBY FRONTEND API",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                text.Append(" This API version has been deprecated.");
            }

            if (description.SunsetPolicy is SunsetPolicy policy)
            {
                if (policy.Date is DateTimeOffset when)
                {
                    text.Append(" The API will be sunset on ")
                        .Append(when.Date.ToShortDateString())
                        .Append('.');
                }

                if (policy.HasLinks)
                {
                    text.AppendLine();

                    for (var i = 0; i < policy.Links.Count; i++)
                    {
                        var link = policy.Links[i];

                        if (link.Type == "text/html")
                        {
                            text.AppendLine();

                            if (link.Title.HasValue)
                            {
                                text.Append(link.Title.Value).Append(": ");
                            }

                            text.Append(link.LinkTarget.OriginalString);
                        }
                    }
                }
            }

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