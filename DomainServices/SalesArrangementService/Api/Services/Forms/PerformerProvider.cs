using CIS.Core.Attributes;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.UserService.Clients;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

[ScopedService, SelfService]
internal class PerformerProvider
{
    private readonly ICaseServiceClient _caseService;
    private readonly IUserServiceClient _userService;

    public PerformerProvider(ICaseServiceClient caseService, IUserServiceClient userService)
    {
        _caseService = caseService;
        _userService = userService;
    }

    public async Task<int?> LoadPerformerUserId(long caseId, CancellationToken cancellationToken)
    {
        var newestTask = await LoadNewestTask(caseId, cancellationToken);

        if (newestTask is null || string.IsNullOrWhiteSpace(newestTask.PerformerLogin))
            return default;

        var user = await _userService.GetUserByLogin(newestTask.PerformerLogin, cancellationToken);

        return user.Id;
    }

    private async Task<WorkflowTask?> LoadNewestTask(long caseId, CancellationToken cancellationToken)
    {
        var tasks = await _caseService.GetTaskList(caseId, cancellationToken);

        return tasks.Where(t => t is { TaskTypeId: 7, Cancelled: false }).MaxBy(x => x.CreatedOn);
    }
}