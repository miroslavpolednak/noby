using System.Reflection;
using FOMS.Api.Endpoints.Codebooks.GetAll;
using FOMS.Api.Endpoints.Codebooks.GetAll.CodebookMap;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FOMS.Api.StartupExtensions;

internal static class FomsSwagger
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
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "NOBY FRONTEND API", Version = "v1" });

            // zapojení rozšířených anotací nad controllery
            x.EnableAnnotations();
            //x.UseOneOfForPolymorphism();

            // všechny parametry budou camel case
            x.DescribeAllParametersInCamelCase();
            x.UseInlineDefinitionsForEnums();

            x.CustomSchemaIds(type => type.ToString());

            // generate the XML docs that'll drive the swagger docs
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName(typeof(Program))));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DomainServices.CodebookService.Contracts.xml"));

            x.SchemaFilter<CodebookGetAllSchemaFilter>(codebookMap);
        });

        // Adds FluentValidationRules staff to Swagger.
        builder.Services.AddFluentValidationRulesToSwagger();

        return builder;
    }

    private class CodebookGetAllSchemaFilter : ISchemaFilter
    {
        private readonly List<Type> _getAllResponsePossibleTypes;

        public CodebookGetAllSchemaFilter(CodebookMap codebookMap)
        {
            _getAllResponsePossibleTypes = codebookMap.GroupBy(c => c.ReturnType, c => c.ReturnType).Select(c => c.First()).ToList();
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(GetAllResponseItem))
                return;

            var codebookCollectionProperty = schema.Properties[nameof(GetAllResponseItem.Codebook).ToLowerInvariant()];

            foreach (var type in _getAllResponsePossibleTypes)
            {
                codebookCollectionProperty.Items.OneOf.Add(context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository));
            }
        }
    }
}

