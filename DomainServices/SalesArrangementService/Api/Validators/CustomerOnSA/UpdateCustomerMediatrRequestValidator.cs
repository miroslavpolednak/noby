using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateCustomerMediatrRequestValidator
    : AbstractValidator<Dto.UpdateCustomerMediatrRequest>
{
    public UpdateCustomerMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CustomerOnSAId)
            .GreaterThan(0)
            .WithMessage("CustomerOnSAId must be > 0").WithErrorCode("13000");
    }
}