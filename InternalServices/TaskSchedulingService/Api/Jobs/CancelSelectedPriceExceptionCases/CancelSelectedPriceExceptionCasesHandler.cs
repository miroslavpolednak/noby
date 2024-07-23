using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelSelectedPriceExceptionCases;

internal sealed class CancelSelectedPriceExceptionCasesHandler(
    DomainServices.CaseService.Clients.IMaintananceClient _maintananceClient, 
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService, 
    TimeProvider _timeProvider, 
    ILogger<CancelSelectedPriceExceptionCasesHandler> _logger)
        : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        var d = _timeProvider.GetLocalNow().Date.AddDays(-45);
        var list = await _maintananceClient.GetConfirmedPriceExceptionsRequest(d, cancellationToken);

        _logger.CasesToCancel(list.Count, d);

        foreach (var priceException in list)
        {
            _logger.TryToCancelCase(priceException.CaseId);

            try
            {
                await _caseService.CancelTask(priceException.CaseId, priceException.TaskIdSB, cancellationToken);

                await _maintananceClient.DeleteConfirmedPriceException(priceException.CaseId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.FailedToCancelCase(priceException.CaseId, ex);
            }
        }
    }
}
