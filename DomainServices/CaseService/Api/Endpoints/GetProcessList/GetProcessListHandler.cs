using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.GetProcessList;

internal class GetProcessListHandler : IRequestHandler<GetProcessListRequest, GetProcessListResponse>
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
        await CheckIfCaseExists(request.CaseId, cancellationToken);

        var sbRequest = new FindByCaseIdRequest
        {
            HeaderLogin = await _commonDataProvider.GetCurrentLogin(cancellationToken),
            CaseId = request.CaseId,
            TaskStates = await _commonDataProvider.GetValidTaskStateIds(cancellationToken),
            SearchPattern = "MainLoanProcessTasks"
        };

        var foundTasks = await _sbWebApiClient.FindTasksByCaseId(sbRequest, cancellationToken);

        return new GetProcessListResponse { Processes = { foundTasks.Tasks.Select(t => t.ToProcessTask()) } };
    }

    private async Task CheckIfCaseExists(long caseId, CancellationToken cancellationToken)
    {
        var caseExists = await _dbContext.Cases.AnyAsync(c => c.CaseId == caseId, cancellationToken);

        if (caseExists)
            return;

        throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, caseId);
    }
}