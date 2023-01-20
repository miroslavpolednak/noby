using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangement;

internal class UpdateSalesArrangementRequestValidator
    : AbstractValidator<Contracts.UpdateSalesArrangementRequest>
{
    public UpdateSalesArrangementRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("18010");

        RuleFor(t => t.SalesArrangementSignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return id.HasValue ? (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id) : true;
            })
            .WithMessage("SalesArrangementSignatureTypeId not found").WithErrorCode("99999"); // TODO: Error code
    }
}
