using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class LegalPersonValidator : AbstractValidator<LegalPerson>
{
    public LegalPersonValidator()
    {
        RuleFor(person => person.Name)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.NameRequired)
            .MaximumLength(255)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.NameLengthLimitExceeded);
    }
}