using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;

namespace DomainServices.CaseService.Api.Services;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class SbWebApiCommonDataProvider
{
    private readonly ICodebookServiceClients _codebookService;

    public SbWebApiCommonDataProvider(ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
    }

    public async Task<ICollection<int>> GetValidTaskStateIds(CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStates(cancellationToken);

        return taskStates.Where(i => !i.Flag.HasFlag(EWorkflowTaskStateFlag.Inactive)).Select(i => i.Id).ToList();
    }
}