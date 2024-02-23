using Microsoft.OpenApi.Models;
using NOBY.Infrastructure.Security.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Infrastructure.Swagger;

public sealed class ApplySwaggerNobyAttributes 
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool skipCaseOwnerStateAndProduct = context.MethodInfo.GetCustomAttributes(typeof(NobySkipCaseStateAndProductSAValidationAttribute), false)?.Any() ?? false;
        bool skipCaseOwner = context.MethodInfo.GetCustomAttributes(typeof(NobySkipCaseOwnerValidationAttribute), false)?.Any() ?? false;

        // prava
        var authorizeAttributes = context.MethodInfo.GetCustomAttributes(typeof(NobyAuthorizeAttribute), false);
        if (authorizeAttributes is not null && authorizeAttributes.Length != 0)
        {
            string perms = string.Join("<br/><font color=\"lightgrey\">[AND]</font><br/>", authorizeAttributes.Select(t => string.Join(" <font color=\"lightgrey\">[AND]</font> ", ((NobyAuthorizeAttribute)t).RequiredPermissions)));
            operation.Description += $"{_requiredPermissionsLabel}<br/>{perms}";
        }

        if (skipCaseOwnerStateAndProduct || skipCaseOwner)
        {
            List<string> skips = new();
            if (skipCaseOwner) skips.Add("NobySkipCaseOwnerValidation");
            if (skipCaseOwnerStateAndProduct) skips.Add("NobySkipCaseOwnerStateAndProductSAValidation");
            operation.Description = $"{operation.Description}<br/><br/><strong style=\"color:#61affe;\">Permission check middleware attributes:</strong><br/>{string.Join(";", skips)}";
        }

        // EAcko
        if (context.MethodInfo.GetCustomAttributes(typeof(SwaggerEaDiagramAttribute), false)?.FirstOrDefault() is SwaggerEaDiagramAttribute eaAttribute)
        {
            operation.Description += $"{operation.Description}<br/><br/><strong style=\"color:#61affe;\">EA Diagram</strong><br/><a href=\"{eaAttribute.DiagramUrl}\">{eaAttribute.DiagramUrl}</a>";
        }
    }

    private const string _requiredPermissionsLabel = "<br/><br/><strong style=\"color:red;\">Required permissions</strong>";
}