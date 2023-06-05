using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementState;

internal sealed class UpdateSalesArrangementStateRequestValidator
    : AbstractValidator<Contracts.UpdateSalesArrangementStateRequest>
{
    public UpdateSalesArrangementStateRequestValidator(CodebookService.Clients.ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);

        RuleFor(t => t.State)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementStateIsEmpty);

        RuleFor(t => t.State)
            .MustAsync(async (t, cancellationToken) => (await codebookService.SalesArrangementStates(cancellationToken)).Any(c => c.Id == t))
            .WithErrorCode(ErrorCodeMapper.SalesArrangementStateNotFound);
    }
}
