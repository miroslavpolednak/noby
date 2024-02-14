
namespace DomainServices.SalesArrangementService.Clients.Services;

internal sealed class MaintananceService
    : IMaintananceService
{
    public async Task<long[]> GetCancelCaseJobIds(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCancelCaseJobIdsAsync(new(), cancellationToken: cancellationToken);
        return result.CaseId?.ToArray() ?? Array.Empty<long>();
    }

    private readonly Contracts.MaintananceService.MaintananceServiceClient _service;

    public MaintananceService(Contracts.MaintananceService.MaintananceServiceClient service)
    {
        _service = service;
    }
}
