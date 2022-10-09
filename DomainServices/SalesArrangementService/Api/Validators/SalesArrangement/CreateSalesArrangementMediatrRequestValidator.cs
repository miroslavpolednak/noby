using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class CreateSalesArrangementMediatrRequestValidator 
    : AbstractValidator<Dto.CreateSalesArrangementMediatrRequest>
{
    public CreateSalesArrangementMediatrRequestValidator(DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("18008");

        RuleFor(t => t.Request.SalesArrangementTypeId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementTypeId must be > 0").WithErrorCode("18009");

        RuleFor(t => t.Request.SalesArrangementSignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return id.HasValue ? (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id) : true;
            })
            .WithMessage("SalesArrangementSignatureTypeId not found").WithErrorCode("99999"); // TODO: Error code
    }
}

