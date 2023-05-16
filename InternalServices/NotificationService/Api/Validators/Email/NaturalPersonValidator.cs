using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class NaturalPersonValidator : AbstractValidator<NaturalPerson>
{
    public NaturalPersonValidator()
    {
        RuleFor(person => person.FirstName)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.FirstNameRequired)
            .MaximumLength(40)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.FirstNameLengthLimitExceeded);

        RuleFor(person => person.MiddleName)
            .MaximumLength(40)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.MiddleNameLengthLimitExceeded);

        RuleFor(person => person.Surname)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SurnameRequired)
            .MaximumLength(80)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SurnameLengthLimitExceeded);
    }
}