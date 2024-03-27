using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using ExternalServices.SbWebApi.Dto.FindTasks;
using ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.v1.GetProcessList;

internal sealed class GetProcessListHandler
    : IRequestHandler<GetProcessListRequest, GetProcessListResponse>
{
    private readonly CaseServiceDbContext _dbContext;
    private readonly ISbWebApiClient _sbWebApiClient;
    private readonly ICodebookServiceClient _codebookService;

    public GetProcessListHandler(CaseServiceDbContext dbContext, ISbWebApiClient sbWebApiClient, ICodebookServiceClient codebookService)
    {
        _dbContext = dbContext;
        _sbWebApiClient = sbWebApiClient;
        _codebookService = codebookService;
    }

    public async Task<GetProcessListResponse> Handle(GetProcessListRequest request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Cases.AnyAsync(c => c.CaseId == request.CaseId, cancellationToken))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
        }

        var taskStateIds = (await _codebookService.WorkflowTaskStates(cancellationToken))
            .Select(i => i.Id)
            .ToList();

        var sbRequest = new FindByCaseIdRequest
        {
            CaseId = request.CaseId,
            TaskStates = taskStateIds,
            SearchPattern = "MainLoanProcessTasks"
        };

        var foundTasks = await _sbWebApiClient.FindTasksByCaseId(sbRequest, cancellationToken);

        return new GetProcessListResponse
        {
            Processes = { foundTasks.Select(taskData => taskData.ToProcessTask()) }
        };
    }
}