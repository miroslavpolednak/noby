using Microsoft.OpenApi.Models;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Api.Endpoints.SalesArrangement.GetDetail;

internal class GetDetailSwaggerSchema : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(GetDetailResponse))
            return;

        var possibleTypes = new[]
        {
            typeof(ParametersMortgage),
            typeof(ParametersDrawing),
            typeof(Dto.HUBNDetail),
            typeof(Dto.GeneralChangeDetail)
        };

        foreach (var type in possibleTypes)
        {
            var typeSchema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

            schema.Properties[nameof(GetDetailResponse.Parameters).ToLowerInvariant()].OneOf.Add(typeSchema);
        }
    }
}