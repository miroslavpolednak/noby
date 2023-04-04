using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.CreateSalesArrangement;

internal sealed class CreateSalesArrangementRequestValidator
    : AbstractValidator<Contracts.CreateSalesArrangementRequest>
{
    public CreateSalesArrangementRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.SalesArrangementTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementTypeIdIsEmpty);

        RuleFor(t => t.SalesArrangementSignatureTypeId)
            .MustAsync(async (id, cancellation) =>
            {
                return id.HasValue ? (await codebookService.SignatureTypes(cancellation)).Any(t => t.Id == id) : true;
            })
            .WithErrorCode(ErrorCodeMapper.SalesArrangementSignatureTypeIdNotFound);

        RuleFor(t => t.SalesArrangementTypeId)
            .MustAsync(async (t, cancellationToken) => (await codebookService.SalesArrangementTypes(cancellationToken)).Any(c => c.Id == t))
            .WithErrorCode(ErrorCodeMapper.SalesArrangementTypeNotFound);
    }
}

