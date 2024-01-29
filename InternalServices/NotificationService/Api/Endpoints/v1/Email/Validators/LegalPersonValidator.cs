using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email.Validators;

public class LegalPersonValidator : AbstractValidator<LegalPerson>
{
    public LegalPersonValidator()
    {
        RuleFor(person => person.Name)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.NameRequired)
            .MaximumLength(255)
                .WithErrorCode(ErrorCodeMapper.NameLengthLimitExceeded);
    }
}