using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Infrastructure.Swagger;

public class ApplySwaggerNobyAttributes 
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // prava
        var authorizeAttributes = context.MethodInfo.GetCustomAttributes(typeof(Security.NobyAuthorizeAttribute), false);
        if (authorizeAttributes is not null && authorizeAttributes.Any())
        {
            string perms = string.Join("<br/><font color=\"lightgrey\">[AND]</font><br/>", authorizeAttributes.Select(t => string.Join(" <font color=\"lightgrey\">[OR]</font> ", ((Security.NobyAuthorizeAttribute)t).RequiredPermissions)));
            operation.Description = $"{operation.Description}<br/><br/><strong style=\"color:red;\">Required permissions</strong><br/>{perms}";
        }
        
        // EAcko
        if (context.MethodInfo.GetCustomAttributes(typeof(SwaggerEaDiagramAttribute), false)?.FirstOrDefault() is SwaggerEaDiagramAttribute eaAttribute)
        {
            operation.Description = $"{operation.Description}<br/><br/><strong style=\"color:#61affe;\">EA Diagram</strong><br/><a href=\"{eaAttribute.DiagramUrl}\">{eaAttribute.DiagramUrl}</a>";
        }
    }
}