using Microsoft.AspNetCore.Mvc;

namespace CIS.InternalServices.NotificationService.Api.ErrorHandling;

public static class ApiBehaviourExtensions
{
    public static void AddCustomInvalidModelStateResponseFactory(this ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(new CustomValidationResponse(context));
    }
}