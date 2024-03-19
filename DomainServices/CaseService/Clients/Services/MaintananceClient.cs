namespace DomainServices.CaseService.Clients.Services;

internal sealed class MaintananceClient
    : IMaintananceClient
{
    public async Task<List<long>> GetConfirmedPriceExceptionsRequest(DateTime olderThan, CancellationToken cancellationToken = default)
        => (await _service.GetConfirmedPriceExceptionsAsync(new Contracts.GetConfirmedPriceExceptionsRequest
        {
            OlderThan = olderThan
        }, cancellationToken: cancellationToken)).CaseId.ToList();

    public async Task DeleteConfirmedPriceException(long caseId, CancellationToken cancellationToken = default)
        => await _service.DeleteConfirmedPriceExceptionAsync(new Contracts.DeleteConfirmedPriceExceptionRequest
        {
            CaseId = caseId
        }, cancellationToken: cancellationToken);

    private readonly Contracts.MaintananceService.MaintananceServiceClient _service;

    public MaintananceClient(Contracts.MaintananceService.MaintananceServiceClient service)
        => _service = service;
}
