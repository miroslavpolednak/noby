using Microsoft.OpenApi.Models;
using NOBY.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NOBY.Api.Endpoints.RealEstateValuation;

internal sealed class ApplySwaggerRealEstateValuationAttributes
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool reqPermSection = false;

        // case owner info
        if (context.MethodInfo.GetCustomAttributes(typeof(RealEstateValuationStateValidationAttribute), false).Any())
        {
            if (!reqPermSection)
                operation.Description += _requiredPermissionsLabel;
            operation.Description += "<br/>RealEstateValuationStateValidation()";
            reqPermSection = true;
        }
    }

    private const string _requiredPermissionsLabel = "<br/><br/><strong style=\"color:red;\">Required permissions from RealEstateValuation namespace</strong>";
}
