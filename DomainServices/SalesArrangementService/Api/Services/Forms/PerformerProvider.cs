using CIS.Core.Attributes;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.UserService.Clients;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal class PerformerProvider
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

    public async Task<int?> LoadPerformerUserId(long caseId, CancellationToken cancellationToken)
    {
        var newestTask = await LoadNewestTask(caseId, cancellationToken);

        if (newestTask is null || string.IsNullOrWhiteSpace(newestTask.PerformerLogin))
            return default;

        try
        {
            var op = await _codebookService.GetOperator(newestTask.PerformerLogin, cancellationToken);

            if (op.PerformerCode == default)
                return default;
        }
        catch (CisNotFoundException)
        {
            return default; //Ignore
        }

        var user = await _userService.GetUser(new CIS.Foms.Types.UserIdentity(newestTask.PerformerLogin, CIS.Foms.Enums.UserIdentitySchemes.Mpad), cancellationToken);

        return user.UserId;
    }

    private async Task<WorkflowTask?> LoadNewestTask(long caseId, CancellationToken cancellationToken)
    {
        var tasks = await _caseService.GetTaskList(caseId, cancellationToken);

        return tasks.Where(t => t is { TaskTypeId: 7, Cancelled: false }).MaxBy(x => x.CreatedOn);
    }
}