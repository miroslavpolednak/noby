using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Clients.Services;

internal sealed class MaintananceClient(Contracts.MaintananceService.MaintananceServiceClient _service)
    : IMaintananceClient
{
    public async Task<List<ConfirmedPriceException>> GetConfirmedPriceExceptionsRequest(DateTime olderThan, CancellationToken cancellationToken = default)
        => (await _service.GetConfirmedPriceExceptionsAsync(new Contracts.GetConfirmedPriceExceptionsRequest
        {
            OlderThan = olderThan
        }, cancellationToken: cancellationToken)).ConfirmedPriceExp.ToList();

    public async Task DeleteConfirmedPriceException(long caseId, CancellationToken cancellationToken = default)
        => await _service.DeleteConfirmedPriceExceptionAsync(new Contracts.DeleteConfirmedPriceExceptionRequest
        {
            CaseId = caseId
        }, cancellationToken: cancellationToken);
}
