using CIS.Core.Security;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;
using DomainServices.UserService.Clients;
using DomainServices.UserService.Contracts;

namespace DomainServices.CaseService.Api.Services;

internal class SbWebApiCommonDataProvider
{
    private readonly ICodebookServiceClients _codebookService;
    private readonly IUserServiceClient _userService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public SbWebApiCommonDataProvider(ICodebookServiceClients codebookService, IUserServiceClient userService, ICurrentUserAccessor currentUserAccessor)
    {
        _codebookService = codebookService;
        _userService = userService;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<string> GetCurrentLogin(CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(_currentUserAccessor.User!.Id, cancellationToken);

        return GetLogin(user);
    }

    public async Task<ICollection<int>> GetValidTaskStateIds(CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStates(cancellationToken);

        return taskStates.Where(i => !i.Flag.HasFlag(EWorkflowTaskStateFlag.Inactive)).Select(i => i.Id).ToList();
    }

    private static string GetLogin(User user)
    {
        if (!string.IsNullOrWhiteSpace(user.CPM) && !string.IsNullOrWhiteSpace(user.ICP))
        {
            // "login": "CPM: 99811022 ICP: 128911022"
            return $"CPM: {user.CPM} ICP: {user.ICP}";
        }

        var identity = user.UserIdentifiers.FirstOrDefault()?.Identity;

        return identity ?? string.Empty;
    }
}