using Microsoft.OpenApi.Models;
using NOBY.Api.Endpoints.SalesArrangement.Dto;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement;

internal sealed class GetSalesArrangementSwaggerSchema : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(GetSalesArrangementResponse))
            return;

        var possibleTypes = new[]
        {
            typeof(ParametersMortgage),
            typeof(ParametersDrawing),
            typeof(Dto.HUBNDetail),
            typeof(Dto.GeneralChangeDetail),
            typeof(Dto.CustomerChangeDetail),
            typeof(Dto.CustomerChange3602Detail)
        };

        foreach (var type in possibleTypes)
        {
            var typeSchema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

            schema.Properties[nameof(GetSalesArrangementResponse.Parameters).ToLowerInvariant()].OneOf.Add(typeSchema);
        }
    }
}