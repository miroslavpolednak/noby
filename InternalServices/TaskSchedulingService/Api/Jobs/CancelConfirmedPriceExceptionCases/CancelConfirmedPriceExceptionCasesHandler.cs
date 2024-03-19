using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CancelConfirmedPriceExceptionCases;

internal sealed class CancelConfirmedPriceExceptionCasesHandler
    : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        var d = _timeProvider.GetLocalNow().Date.AddDays(-45);
        var list = await _maintananceClient.GetConfirmedPriceExceptionsRequest(d, cancellationToken);

        _logger.CasesToCancel(list.Count, d);

        foreach (long caseId in list)
        {
            _logger.TryToCancelCase(caseId);

            try
            {
                await _caseService.CancelCase(caseId, false, cancellationToken);
                
                await _maintananceClient.DeleteConfirmedPriceException(caseId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.FailedToCancelCase(caseId, ex);
            }
        }
    }

    private readonly ILogger<CancelConfirmedPriceExceptionCasesHandler> _logger;
    private readonly TimeProvider _timeProvider;
    private readonly DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService;
    private readonly DomainServices.CaseService.Clients.IMaintananceClient _maintananceClient;

    public CancelConfirmedPriceExceptionCasesHandler(DomainServices.CaseService.Clients.IMaintananceClient maintananceClient, DomainServices.CaseService.Clients.v1.ICaseServiceClient caseService, TimeProvider timeProvider, ILogger<CancelConfirmedPriceExceptionCasesHandler> logger)
    {
        _maintananceClient = maintananceClient;
        _caseService = caseService;
        _timeProvider = timeProvider;
        _logger = logger;
    }
}
