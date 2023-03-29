using DomainServices.CaseService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class CaseServiceWrapper : IServiceWrapper
{
    private readonly ICaseServiceClient _caseService;

    public CaseServiceWrapper(ICaseServiceClient caseService)
    {
        _caseService = caseService;
    }

    public DataSource DataSource => DataSource.CaseService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateCaseId();

        data.Case = await _caseService.GetCaseDetail(input.CaseId!.Value, cancellationToken);
    }
}