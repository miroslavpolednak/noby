using DomainServices.CaseService.Clients;

namespace CIS.InternalServices.DataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CaseServiceWrapper : IServiceWrapper
{
    private readonly ICaseServiceClient _caseService;

    public CaseServiceWrapper(ICaseServiceClient caseService)
    {
        _caseService = caseService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.CaseId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.CaseId));

        data.Case = await _caseService.GetCaseDetail(input.CaseId.Value, cancellationToken);
    }
}