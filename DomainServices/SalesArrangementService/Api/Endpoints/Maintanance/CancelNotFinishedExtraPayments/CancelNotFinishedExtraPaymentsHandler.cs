using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.Maintanance.CancelNotFinishedExtraPayments;

internal sealed class CancelNotFinishedExtraPaymentsHandler(
    ILogger<CancelNotFinishedExtraPaymentsHandler> _logger,
    SalesArrangementServiceDbContext _dbContext,
    ICaseServiceClient _caseService,
    IMediator _mediator)
    : IRequestHandler<CancelNotFinishedExtraPaymentsRequest, Empty>
{
    public async Task<Empty> Handle(CancelNotFinishedExtraPaymentsRequest request, CancellationToken cancellationToken)
    {
        var newExtraPaymentsSA = await _dbContext.SalesArrangements
                                    .Where(s => s.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageExtraPayment
                                                && s.State == (int)EnumSalesArrangementStates.NewArrangement)
                                    .Select(s => new
                                    {
                                        s.SalesArrangementId,
                                        s.CaseId,
                                        s.ProcessId
                                    })
                                    .ToListAsync(cancellationToken);

        foreach (var epSa in newExtraPaymentsSA)
        {
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                var existNonCancelEPTaskInSb = (await _caseService.GetTaskList(epSa.CaseId, cancellationToken))
                    .Any(wf => wf.ProcessId == epSa.ProcessId
                              && wf.TaskTypeId == (int)WorkflowTaskTypes.PriceException
                              && !wf.Cancelled);

                if (!existNonCancelEPTaskInSb)
                {
                    var process = (await _caseService.GetProcessList(epSa.CaseId, cancellationToken))
                                .First(t => t.ProcessId == epSa.ProcessId);

                    if (!process.Cancelled)
                    {
                        await _caseService.CancelTask(epSa.CaseId, process.ProcessIdSb, cancellationToken: cancellationToken);
                    }
                    
                    await _mediator.Send(new UpdateSalesArrangementStateRequest(
                        new() { SalesArrangementId = epSa.SalesArrangementId, State = (int)EnumSalesArrangementStates.Cancelled }),
                        cancellationToken);
                }
            }
            catch (Exception ex)
            {
                // muze se stat, ze task neni nalezen a pak pada zpracovani vsech ostatnich SA
                _logger.CancelNotFinishedExtraPaymentsFailed(epSa.CaseId, ex.Message, ex);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        return new();
    }
}
