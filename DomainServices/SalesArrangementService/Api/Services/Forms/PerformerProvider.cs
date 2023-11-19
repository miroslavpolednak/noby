using CIS.Core.Attributes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.UserService.Clients;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal sealed class PerformerProvider
{
    private readonly ICaseServiceClient _caseService;
    private readonly IUserServiceClient _userService;
    private readonly ICodebookServiceClient _codebookService;

    public PerformerProvider(ICaseServiceClient caseService, IUserServiceClient userService, ICodebookServiceClient codebookService)
    {
        _caseService = caseService;
        _userService = userService;
        _codebookService = codebookService;
    }

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

        return tasks.Where(t => t is { TaskTypeId: 7, Cancelled: false }).MaxBy(x => x.CreatedOn);
    }
}