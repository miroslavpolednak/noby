using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.OpenApi.Models;
using NOBY.Api.Endpoints.Codebooks.CodebookMap;
using NOBY.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Api.StartupExtensions;

internal static class NobySwagger
{
    static string xmlFileName(Type type) => type.GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");

    public static WebApplicationBuilder AddFomsSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        var codebookMap = new CodebookMap();
        builder.Services.AddSingleton<ICodebookMap>(codebookMap);

        // konfigurace pro generátor JSON souboru
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "NOBY FRONTEND API", 
                Version = "v1", 
                Description = "Obecná specifikace <b>error handlingu</b> <ul><li>[https://wiki.kb.cz/pages/viewpage.action?pageId=589534698](https://wiki.kb.cz/pages/viewpage.action?pageId=589534698)</li></ul>Specifikace <b>HTTP hlaviček</b> <ul><li>[https://wiki.kb.cz/pages/viewpage.action?pageId=513345095](https://wiki.kb.cz/pages/viewpage.action?pageId=513345095)</li></ul>"
            });

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
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CIS.Foms.Types.xml"));
            
            x.SchemaFilter<SwaggerOneOfSchemaFilter>();
            x.SchemaFilter<CodebookGetAllSchemaFilter>(codebookMap);
            x.SchemaFilter<EnumValuesDescriptionSchemaFilter>();

            x.OperationFilter<ApplySwaggerNobyAttributes>();
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
}

