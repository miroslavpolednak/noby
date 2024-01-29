using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email.Validators;

internal sealed class NaturalPersonValidator : AbstractValidator<NaturalPerson>
{
    public NaturalPersonValidator()
    {
        RuleFor(person => person.FirstName)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.FirstNameRequired)
            .MaximumLength(40)
                .WithErrorCode(ErrorCodeMapper.FirstNameLengthLimitExceeded);

        RuleFor(person => person.MiddleName)
            .MaximumLength(40)
                .WithErrorCode(ErrorCodeMapper.MiddleNameLengthLimitExceeded);

        RuleFor(person => person.Surname)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.SurnameRequired)
            .MaximumLength(80)
                .WithErrorCode(ErrorCodeMapper.SurnameLengthLimitExceeded);
    }
}