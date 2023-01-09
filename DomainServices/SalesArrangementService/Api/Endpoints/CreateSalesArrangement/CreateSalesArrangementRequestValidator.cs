using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.CreateSalesArrangement;

internal sealed class CreateSalesArrangementRequestValidator
    : AbstractValidator<Contracts.CreateSalesArrangementRequest>
{
    public CreateSalesArrangementRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("18008");

        RuleFor(t => t.SalesArrangementTypeId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementTypeId must be > 0").WithErrorCode("18009");

        RuleFor(t => t.SalesArrangementSignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return id.HasValue ? (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id) : true;
            })
            .WithMessage("SalesArrangementSignatureTypeId not found").WithErrorCode("99999"); // TODO: Error code
    }
}

