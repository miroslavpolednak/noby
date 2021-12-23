using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Case.Handlers;

internal class SearchHandler
    : IRequestHandler<Dto.SearchRequest, Dto.SearchResponse>
{
    public async Task<Dto.SearchResponse> Handle(Dto.SearchRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Search for user {userId}", _userAccessor.User.Id);

        var result = resolveResult(await _caseService.SearchCases(_userAccessor.User.Id, request, request.State, request.Term));

        _logger.LogDebug("Found {records} records", result.Pagination.RecordsTotalSize);

        var productTypes = await _codebookService.ProductInstanceTypes();

        // transform
        return new Dto.SearchResponse
        {
            Rows = result.CaseInstances.Select(t => new Dto.SearchItem
            {
                CaseId = t.CaseId,
                State = t.State,
                ActionRequired = t.ActionRequired,
                ContractNumber = t.ContractNumber,
                TargetAmount = t.TargetAmount,
                CreatedBy = t.Created.UserName,
                CreatedTime = t.Created.DateTime,
                CustomerDateOfBirth = t.DateOfBirthNaturalPerson,
                ProductName = productTypes.First(x => x.Id == t.ProductInstanceType).Name,
                CustomerName = $"{t.FirstNameNaturalPerson} {t.Name}".Trim()
            }).ToList(),
            Pagination = result.Pagination
        };
    }

    private DomainServices.CaseService.Contracts.SearchCasesResponse resolveResult(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<DomainServices.CaseService.Contracts.SearchCasesResponse> r => r.Model,
           _ => throw new NotImplementedException()
       };

    private readonly ILogger<SearchHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public SearchHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<SearchHandler> logger,
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _codebookService = codebookService;
        _userAccessor = userAccessor;
        _logger = logger;
        _caseService = caseService;
    }
}
