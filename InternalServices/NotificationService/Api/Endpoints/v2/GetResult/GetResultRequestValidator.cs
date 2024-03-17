using CIS.InternalServices.NotificationService.Contracts.v2;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.GetResult;

internal sealed class GetResultRequestValidator
    : AbstractValidator<GetResultRequest>
{
    public GetResultRequestValidator()
    {
        RuleFor(request => request.NotificationId)
            .NotEmpty()
            .Must(t => Guid.TryParse(t, out Guid _))
            .WithErrorCode(ErrorCodeMapper.NotificationIdRequired);
    }
}
