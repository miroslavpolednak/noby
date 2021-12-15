using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateCaseDataMediatrRequestValidator : AbstractValidator<Dto.UpdateCaseDataMediatrRequest>
{
    public UpdateCaseDataMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13000");

        RuleFor(t => t.ContractNumber)
            .NotEmpty()
            .WithMessage("ContractNumber is empty").WithErrorCode("13001")
            .Length(10)
            .WithMessage("ContractNumber length != 10").WithErrorCode("13001");
    }
}
