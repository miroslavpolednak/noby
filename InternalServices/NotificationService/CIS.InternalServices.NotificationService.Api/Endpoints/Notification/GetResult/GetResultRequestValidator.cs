using CIS.InternalServices.NotificationService.Contracts.Result;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.GetResult;

public class GetResultRequestValidator : AbstractValidator<ResultGetRequest>
{
    public GetResultRequestValidator()
    {
        RuleFor(request => request.NotificationId)
            .NotEmpty()
            .WithErrorCode(nameof(ResultGetRequest.NotificationId));
    }
}