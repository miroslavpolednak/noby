using CIS.InternalServices.NotificationService.Contracts.Result;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Result;

public class GetResultRequestValidator : AbstractValidator<GetResultRequest>
{
    public GetResultRequestValidator()
    {
        RuleFor(request => request.NotificationId)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Result.NotificationIdNotEmpty)
                .WithMessage($"{nameof(GetResultRequest.NotificationId)} must be not empty.");
    }
}