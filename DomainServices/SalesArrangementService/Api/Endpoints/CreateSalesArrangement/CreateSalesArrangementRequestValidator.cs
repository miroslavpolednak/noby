using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.CreateSalesArrangement;

internal sealed class CreateSalesArrangementRequestValidator
    : AbstractValidator<Contracts.CreateSalesArrangementRequest>
{
    public CreateSalesArrangementRequestValidator(CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.SalesArrangementTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementTypeIdIsEmpty);

        RuleFor(t => t.SalesArrangementTypeId)
            .MustAsync(async (t, cancellationToken) => (await codebookService.SalesArrangementTypes(cancellationToken)).Any(c => c.Id == t))
            .WithErrorCode(ErrorCodeMapper.SalesArrangementTypeNotFound);

        When(t => t.DataCase == Contracts.CreateSalesArrangementRequest.DataOneofCase.Mortgage && t.Mortgage is not null, () =>
        {
            RuleFor(t => t.Mortgage.Agent)
                .Empty()
                .WithErrorCode(ErrorCodeMapper.MortgageAgentIsNotEmpty);
        });
    }
}

