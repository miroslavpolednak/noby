using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class CreateSalesArrangementMediatrRequestValidator : AbstractValidator<Dto.CreateSalesArrangementMediatrRequest>
{
    public CreateSalesArrangementMediatrRequestValidator()
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("Case ID does not exist.").WithErrorCode("16002");

        RuleFor(t => t.Request.SalesArrangementTypeId)
            .GreaterThan(0)
            .WithMessage("OfferInstance ID does not exist.").WithErrorCode("16001");
    }
}

