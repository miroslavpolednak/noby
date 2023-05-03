using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Api.Services;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.Dto.FindTasks;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskListByContract;

internal sealed class GetTaskListByContractHandler
    : IRequestHandler<GetTaskListByContractRequest, GetTaskListResponse>
{
    public async Task<GetTaskListResponse> Handle(GetTaskListByContractRequest request, CancellationToken cancellationToken)
    {
        // overit existenci case u nas
        var caseInstance = await _dbContext.Cases
            .AsNoTracking()
            .Where(t => t.ContractNumber == request.ContractNumber)
            .Select(t => new { t.CaseId })
            .FirstOrDefaultAsync(cancellationToken);

        if (caseInstance is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ContractNumberNotFound);
        }

        return await _commonDataProvider.GetAndUpdateTasksList(caseInstance.CaseId, async (taskStateIds) =>
        {
            var sbRequest = new FindByContractNumberRequest
            {
                ContractNumber = request.ContractNumber,
                TaskStates = taskStateIds,
                SearchPattern = "LoanProcessSubtasks"
            };
            return await _sbWebApiClient.FindTasksByContractNumber(sbRequest, cancellationToken);
        }, cancellationToken);
    }

    private readonly CaseServiceDbContext _dbContext;
    private readonly ISbWebApiClient _sbWebApiClient;
    private readonly SbWebApiCommonDataProvider _commonDataProvider;

    public GetTaskListByContractHandler(
        CaseServiceDbContext dbContext,
        SbWebApiCommonDataProvider commonDataProvider,
        ISbWebApiClient sbWebApiClient)
    {
        _dbContext = dbContext;
        _commonDataProvider = commonDataProvider;
        _sbWebApiClient = sbWebApiClient;
    }
}
