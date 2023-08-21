using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Contracts.Result;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Result.Validators;

public class GetResultRequestValidator : AbstractValidator<GetResultRequest>
{
    public GetResultRequestValidator()
    {
        RuleFor(request => request.NotificationId)
            .NotEmpty()
            .WithErrorCode(ErrorHandling.ErrorCodeMapper.NotificationIdRequired);
    }
}