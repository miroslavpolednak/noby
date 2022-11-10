using Microsoft.OpenApi.Models;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal class SalesArrangementParametersSwagerSchema : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(UpdateParametersRequest))
            return;

        var possibleTypes = new[]
        {
            typeof(ParametersMortgage),
            typeof(ParametersDrawing)
        };

        foreach (var type in possibleTypes)
        {
            var typeSchema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

            schema.Properties[nameof(UpdateParametersRequest.Parameters).ToLowerInvariant()].OneOf.Add(typeSchema);
        }
    }
}
