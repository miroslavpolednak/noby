using CIS.Core.Security;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;
using DomainServices.UserService.Clients;
using ExternalServices.EasSimulationHT.V1;

namespace DomainServices.CaseService.Api.Endpoints.GetProcessList;

internal class GetProcessListHandler : IRequestHandler<GetProcessListRequest, GetProcessListResponse>
{
    private readonly CaseServiceDbContext _dbContext;
    private readonly IUserServiceClient _userService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IEasSimulationHTClient _easSimulationService;
    private readonly ICodebookServiceClients _codebookService;

    public GetProcessListHandler(CaseServiceDbContext dbContext, IUserServiceClient userService, ICurrentUserAccessor currentUserAccessor, IEasSimulationHTClient easSimulationService, ICodebookServiceClients codebookService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _currentUserAccessor = currentUserAccessor;
        _easSimulationService = easSimulationService;
        _codebookService = codebookService;
    }

    public async Task<GetProcessListResponse> Handle(GetProcessListRequest request, CancellationToken cancellationToken)
    {
        await CheckIfCaseExists(request.CaseId, cancellationToken);

        var header = new EasSimulationHT.EasSimulationHTWrapper.WFS_Header { system = "NOBY", login = await GetLogin(cancellationToken) };
        var message = new EasSimulationHT.EasSimulationHTWrapper.WFS_Find_ByCaseId
        {
            case_id = (int)request.CaseId,
            task_state = await GetActiveTaskStateIds(cancellationToken),
            search_pattern = "MainLoanProcessTasks",
        };

        var tasks = await _easSimulationService.FindTasks(header, message, cancellationToken);

        return new GetProcessListResponse { Processes = { tasks.Select(t => t.ToProcessTask()) } };
    }

    private async Task CheckIfCaseExists(long caseId, CancellationToken cancellationToken)
    {
        var caseExists = await _dbContext.Cases.AnyAsync(c => c.CaseId == caseId, cancellationToken);

        if (caseExists)
            return;

        throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, caseId);
    }

    private async Task<string> GetLogin(CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(_currentUserAccessor.User!.Id, cancellationToken);

        if (!string.IsNullOrWhiteSpace(user.CPM) && !string.IsNullOrWhiteSpace(user.ICP))
        {
            // "login": "CPM: 99811022 ICP: 128911022"
            return $"CPM: {user.CPM} ICP: {user.ICP}";
        }

        var identity = user.UserIdentifiers.FirstOrDefault()?.Identity;

        return identity ?? string.Empty;
    }

    private async Task<int[]> GetActiveTaskStateIds(CancellationToken cancellationToken)
    {
        var taskStates = await _codebookService.WorkflowTaskStates(cancellationToken);

        return taskStates.Where(i => !i.Flag.HasFlag(EWorkflowTaskStateFlag.Inactive))
                         .Select(i => i.Id)
                         .ToArray();
    }
}