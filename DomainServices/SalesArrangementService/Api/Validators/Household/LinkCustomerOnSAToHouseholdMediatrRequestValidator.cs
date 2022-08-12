using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators.Household;

internal class LinkCustomerOnSAToHouseholdMediatrRequestValidator
    : AbstractValidator<Dto.LinkCustomerOnSAToHouseholdMediatrRequest>
{
    public LinkCustomerOnSAToHouseholdMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CustomerOnSAId1)
            .NotNull()
            .When(t => t.Request.CustomerOnSAId2.HasValue)
            .WithMessage("CustomerOnSAId1 is not set although CustomerOnSAId2 is.").WithErrorCode("16056");
    }
}
