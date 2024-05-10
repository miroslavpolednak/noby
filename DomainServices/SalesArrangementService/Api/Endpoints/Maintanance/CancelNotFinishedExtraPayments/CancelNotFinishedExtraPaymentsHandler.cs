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
                                    .Where(s => s.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageExtraPayment
                                                && s.State == (int)SalesArrangementStates.NewArrangement)
                                    .Select(s => new
                                    {
                                        s.SalesArrangementId,
                                        s.CaseId,
                                        s.ProcessId
                                    })
                                    .ToListAsync(cancellationToken);

        foreach (var epSa in newExtraPaymentsSA)
        {
            var existNonCancelEPTaskInSb = (await _caseService.GetTaskList(epSa.CaseId, cancellationToken))
                .Any(wf => wf.ProcessId == epSa.ProcessId
                          && wf.TaskTypeId == (int)WorkflowTaskTypes.PriceException
                          && !wf.Cancelled);

            if (!existNonCancelEPTaskInSb)
            {
                var process = (await caseService.GetProcessList(epSa.CaseId, cancellationToken))
                            .First(t => t.ProcessId == epSa.ProcessId);

                await _caseService.CancelTask(epSa.CaseId, process.ProcessIdSb, cancellationToken: cancellationToken);

                await _mediator.Send(new UpdateSalesArrangementStateRequest(
                    new() { SalesArrangementId = epSa.SalesArrangementId, State = (int)SalesArrangementStates.Cancelled }),
                    cancellationToken);
            }
        }

        return new();
    }
}
