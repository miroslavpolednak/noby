using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.RegularExpressions;

namespace NOBY.Infrastructure.Swagger;

public partial class NewLineReplacementFilter : IOperationFilter, ISchemaFilter
{
    private const string Replacement = "<br />";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!string.IsNullOrWhiteSpace(operation.Summary))
            operation.Summary = NewLineReplacementRegex().Replace(operation.Summary, Replacement);

        if (!string.IsNullOrWhiteSpace(operation.Description))
            operation.Description = NewLineReplacementRegex().Replace(operation.Description, Replacement);
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (string.IsNullOrWhiteSpace(schema.Description))
            return;

        schema.Description = NewLineReplacementRegex().Replace(schema.Description, Replacement);
    }

    [GeneratedRegex(@"<br\s?\/?>\r\n|\r\n")]
    private static partial Regex NewLineReplacementRegex();
}