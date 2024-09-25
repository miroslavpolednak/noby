using CIS.Core.Attributes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.UserService.Clients.v1;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal sealed class PerformerProvider(
    ICaseServiceClient _caseService, 
    IUserServiceClient _userService, 
    ICodebookServiceClient _codebookService)
{
    public async Task SetDynamicValuesPerformerUserId(long caseId, IEnumerable<DynamicFormValues> dynamicFormValues, CancellationToken cancellationToken)
    {
        var loanDynamicValue = dynamicFormValues.First(d => d.DocumentTypeId == (int)DocumentTypes.ZADOSTHU);

        loanDynamicValue.PerformerUserId = await LoadPerformerUserId(caseId, cancellationToken);
    }

    private async Task<int?> LoadPerformerUserId(long caseId, CancellationToken cancellationToken)
    {
        var newestTask = await LoadNewestTask(caseId, cancellationToken);

        if (newestTask is null || string.IsNullOrWhiteSpace(newestTask.PerformerLogin))
            return default;

        try
        {
            var op = await _codebookService.GetOperator(newestTask.PerformerLogin, cancellationToken);

            if (op.PerformerCode == default)
                return default;

            var user = await _userService.GetUser(new SharedTypes.Types.UserIdentity(newestTask.PerformerLogin, UserIdentitySchemes.Kbad), cancellationToken);

            return user.UserId;
        }
        catch (CisNotFoundException)
        {
            return default; //Ignore
        }
    }

    private async Task<WorkflowTask?> LoadNewestTask(long caseId, CancellationToken cancellationToken)
    {
        var tasks = await _caseService.GetTaskList(caseId, cancellationToken);

        return tasks.Where(t => t is { TaskTypeId: 7, Cancelled: false }).MaxBy(x => (DateTime?)x.CreatedOn);
    }
}