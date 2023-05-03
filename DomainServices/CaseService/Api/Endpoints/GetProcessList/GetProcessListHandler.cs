using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.GetProcessList;

internal sealed class GetProcessListHandler : IRequestHandler<GetProcessListRequest, GetProcessListResponse>
{
    private readonly CaseServiceDbContext _dbContext;
    private readonly SbWebApiCommonDataProvider _commonDataProvider;
    private readonly ISbWebApiClient _sbWebApiClient;

    public GetProcessListHandler(CaseServiceDbContext dbContext, SbWebApiCommonDataProvider commonDataProvider, ISbWebApiClient sbWebApiClient)
    {
        _dbContext = dbContext;
        _commonDataProvider = commonDataProvider;
        _sbWebApiClient = sbWebApiClient;
    }

    public async Task<GetProcessListResponse> Handle(GetProcessListRequest request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext.Cases.AnyAsync(c => c.CaseId == request.CaseId, cancellationToken)))
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        }

        var sbRequest = new FindByCaseIdRequest
        {
            CaseId = request.CaseId,
            TaskStates = await _commonDataProvider.GetValidTaskStateIds(cancellationToken),
            SearchPattern = "MainLoanProcessTasks"
        };

        var foundTasks = await _sbWebApiClient.FindTasksByCaseId(sbRequest, cancellationToken);

        return new GetProcessListResponse
        {
            Processes = { foundTasks.Select(taskData => taskData.ToProcessTask()) }
        };
    }
}