using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskList;

internal sealed class GetTaskListHandler
    : IRequestHandler<GetTaskListRequest, GetTaskListResponse>
{
    public async Task<GetTaskListResponse> Handle(GetTaskListRequest request, CancellationToken cancellationToken)
    {
        // check if case exists
        if (!(await _dbContext.Cases.AnyAsync(c => c.CaseId == request.CaseId, cancellationToken)))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        }

        return await _commonDataProvider.GetAndUpdateTasksList(request.CaseId, async (taskStateIds) =>
        {
            var sbRequest = new FindByCaseIdRequest
            {
                CaseId = request.CaseId,
                TaskStates = taskStateIds,
                SearchPattern = "LoanProcessSubtasks"
            };
            return await _sbWebApiClient.FindTasksByCaseId(sbRequest, cancellationToken);
        }, cancellationToken);
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly SbWebApiCommonDataProvider _commonDataProvider;
    private readonly ISbWebApiClient _sbWebApiClient;

    public GetTaskListHandler(
        CaseServiceDbContext dbContext,
        SbWebApiCommonDataProvider commonDataProvider,
        ISbWebApiClient sbWebApiClient)
    {
        _dbContext = dbContext;
        _commonDataProvider = commonDataProvider;
        _sbWebApiClient = sbWebApiClient;
    }
}
