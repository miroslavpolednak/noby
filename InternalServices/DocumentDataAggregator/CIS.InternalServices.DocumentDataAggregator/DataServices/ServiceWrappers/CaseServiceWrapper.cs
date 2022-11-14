using CIS.Core.Results;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

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

        var result = await _caseService.GetCaseDetail(input.CaseId.Value, cancellationToken);

        data.Case = ServiceCallResult.ResolveAndThrowIfError<Case>(result);
    }
}