using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskListByContract;

internal sealed class GetTaskListByContractHandler
    : IRequestHandler<GetTaskListByContractRequest, GetTaskListResponse>
{
    public async Task<GetTaskListResponse> Handle(GetTaskListByContractRequest request, CancellationToken cancellationToken)
    {
        var caseId = await _dbContext.Cases
            .AsNoTracking()
            .Where(t => t.ContractNumber == request.ContractNumber)
            .Select(t => new { t.CaseId })
            .FirstOrDefaultAsync(cancellationToken);

        if (caseId is null)
        {
            ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ContractNumberNotFound);
        }

        return await _mediator.Send(new GetTaskListRequest() { CaseId = caseId.CaseId }, cancellationToken);
    }

    private readonly IMediator _mediator;
    private readonly Database.CaseServiceDbContext _dbContext;

    public GetTaskListByContractHandler(IMediator mediator, Database.CaseServiceDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }
}
