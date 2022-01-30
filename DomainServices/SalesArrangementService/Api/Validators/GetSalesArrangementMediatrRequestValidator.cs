using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class GetSalesArrangementMediatrRequestValidator : AbstractValidator<Dto.GetSalesArrangementMediatrRequest>
{
    public GetSalesArrangementMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangement ID does not exist.").WithErrorCode("16000");
    }
}

