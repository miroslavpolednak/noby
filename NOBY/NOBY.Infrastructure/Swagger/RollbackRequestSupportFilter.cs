using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Infrastructure.Swagger;

public sealed class RollbackRequestSupportFilter
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var param = context.MethodInfo.GetParameters();
        
        if (param is not null && param.Any(t => t.ParameterType.GetInterfaces().Any(t => t.Name == "IRollbackCapable")))
        {
            operation.Description += "<br/><br/><strong>Rollback support</strong>";
        }
    }
}
