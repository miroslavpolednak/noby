using Microsoft.OpenApi.Models;
using NOBY.Infrastructure.Security.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Infrastructure.Swagger;

public class ApplySwaggerNobyAttributes 
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool reqPermSection = false;

        // prava
        var authorizeAttributes = context.MethodInfo.GetCustomAttributes(typeof(NobyAuthorizeAttribute), false);
        if (authorizeAttributes is not null && authorizeAttributes.Any())
        {
            string perms = string.Join("<br/><font color=\"lightgrey\">[AND]</font><br/>", authorizeAttributes.Select(t => string.Join(" <font color=\"lightgrey\">[OR]</font> ", ((NobyAuthorizeAttribute)t).RequiredPermissions)));
            operation.Description += $"{_requiredPermissionsLabel}<br/>{perms}";
            reqPermSection = true;
        }

        // case owner info
        if (context.MethodInfo.GetCustomAttributes(typeof(AuthorizeCaseOwnerAttribute), false).Any())
        {
            if (!reqPermSection)
                operation.Description += _requiredPermissionsLabel;
            operation.Description += "<br/>CaseOwnerCheck()";
            reqPermSection = true;
        }

        // EAcko
        if (context.MethodInfo.GetCustomAttributes(typeof(SwaggerEaDiagramAttribute), false)?.FirstOrDefault() is SwaggerEaDiagramAttribute eaAttribute)
        {
            operation.Description = $"{operation.Description}<br/><br/><strong style=\"color:#61affe;\">EA Diagram</strong><br/><a href=\"{eaAttribute.DiagramUrl}\">{eaAttribute.DiagramUrl}</a>";
        }
    }

    private const string _requiredPermissionsLabel = "<br/><br/><strong style=\"color:red;\">Required permissions</strong>";
}