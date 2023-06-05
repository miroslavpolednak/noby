using Microsoft.OpenApi.Models;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateParameters;

internal sealed class UpdateParametersSwaggerSchema : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(UpdateParametersRequest))
            return;

        var possibleTypes = new[]
        {
            typeof(ParametersMortgage),
            typeof(ParametersDrawing),
            typeof(Dto.HUBNUpdate),
            typeof(Dto.GeneralChangeUpdate),
            typeof(Dto.CustomerChangeUpdate),
            typeof(Dto.CustomerChange3602Update)
        };

        foreach (var type in possibleTypes)
        {
            var typeSchema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

            schema.Properties[nameof(UpdateParametersRequest.Parameters).ToLowerInvariant()].OneOf.Add(typeSchema);
        }
    }
}
