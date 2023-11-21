using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Infrastructure.Swagger;

public sealed class RollbackRequestSupportFilter
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var param = context.MethodInfo.GetParameters();
        var request = param.FirstOrDefault(t => t.ParameterType.GetInterfaces().Any(t => t.Name == "IRollbackCapable"));

        if (request is not null)
        {
            operation.Description += "<br/><br/><strong>Rollback support</strong>";
            
            var description = request.ParameterType.GetCustomAttributes(typeof(RollbackDescriptionAttribute), false);
            if (description is not null && description.Length != 0)
            {
                operation.Description += $"<br/>{((RollbackDescriptionAttribute)description.First()).Description}";
            }
        }
    }
}
