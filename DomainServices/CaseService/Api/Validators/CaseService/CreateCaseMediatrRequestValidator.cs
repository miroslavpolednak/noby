using FluentValidation;

namespace DomainServices.CaseService.Api.Validators.CaseService;

internal class CreateCaseMediatrRequestValidator : AbstractValidator<Dto.CaseService.CreateCaseMediatrRequest>
{
    public CreateCaseMediatrRequestValidator()
    {
        RuleFor(t => t.Request.ProductInstanceType)
            .GreaterThan(0)
            .WithMessage(t => "ProductInstanceType must be greater than 0").WithErrorCode("10001");
    }
}
