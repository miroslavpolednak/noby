using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.Maintanance.CancelNotFinishedExtraPayments;

internal sealed class CancelNotFinishedExtraPaymentsHandler(
    SalesArrangementServiceDbContext dbContext,
    ICaseServiceClient caseService,
    IMediator mediator)
    : IRequestHandler<CancelNotFinishedExtraPaymentsRequest, Empty>
{
    private readonly SalesArrangementServiceDbContext _dbContext = dbContext;
    private readonly ICaseServiceClient _caseService = caseService;
    private readonly IMediator _mediator = mediator;

    public async Task<Empty> Handle(CancelNotFinishedExtraPaymentsRequest request, CancellationToken cancellationToken)
    {
        var newExtraPaymentsSA = await _dbContext.SalesArrangements
                                    .AsNoTracking()
                                    .Where(s => s.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageExtraPayment
                                                && s.State == (int)SalesArrangementStates.NewArrangement)
                                    .ToListAsync(cancellationToken);

        foreach (var epSa in newExtraPaymentsSA)
        {
            var existNonCancelExtraPaymentInSb = (await _caseService.GetTaskList(epSa.CaseId, cancellationToken))
                .Any(wf => wf.ProcessId == epSa.ProcessId
                          && wf.TaskTypeId == (int)WorkflowTaskTypes.PriceException
                          && !wf.Cancelled);

            if (!existNonCancelExtraPaymentInSb)
            {
                var task = (await caseService.GetTaskList(epSa.CaseId, cancellationToken))
                            .First(t => t.TaskId == epSa.ProcessId);

                await _caseService.CancelTask(epSa.CaseId, task.TaskIdSb, cancellationToken: cancellationToken);

                await _mediator.Send(new UpdateSalesArrangementStateRequest(
                    new() { SalesArrangementId = epSa.SalesArrangementId, State = (int)SalesArrangementStates.Cancelled }),
                    cancellationToken);
            }
        }

        return new();
    }
}
