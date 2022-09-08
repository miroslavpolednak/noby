using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FOMS.Api.Endpoints.CustomerIncome;

public class IncomeDataSwaggerSchema : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(CreateIncome.CreateIncomeRequest) && 
            context.Type != typeof(UpdateIncome.UpdateIncomeRequest) &&
            context.Type != typeof(GetIncome.GetIncomeResponse))
            return;

        var possibleTypes = new[]
        {
            typeof(Dto.IncomeDataEmployement),
            typeof(Dto.IncomeDataEntrepreneur),
            typeof(Dto.IncomeDataOther)
        };

        foreach (var type in possibleTypes)
        {
            var typeSchema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

            schema.Properties[nameof(CreateIncome.CreateIncomeRequest.Data).ToLowerInvariant()].OneOf.Add(typeSchema);
        }
    }
}