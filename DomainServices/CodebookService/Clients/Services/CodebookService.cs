using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Clients.Services;

internal partial class CodebookService
    : ICodebookServiceClient
{
    public async Task<GetOperatorResponse> GetOperator(string performerLogin, CancellationToken cancellationToken = default) 
        => await _service.GetOperatorAsync(new GetOperatorRequest()
        {
            PerformerLogin = performerLogin
        }, cancellationToken: cancellationToken);

    public async Task<GetDeveloperResponse> GetDeveloper(int developerId, CancellationToken cancellationToken = default)
        => await _service.GetDeveloperAsync(new GetDeveloperRequest
        {
            DeveloperId = developerId
        }, cancellationToken: cancellationToken);

    public async Task<GetDeveloperProjectResponse> GetDeveloperProject(int developerId, int developerProjectId, CancellationToken cancellationToken = default)
        => await _service.GetDeveloperProjectAsync(new GetDeveloperProjectRequest
        {
            DeveloperId = developerId,
            DeveloperProjectId = developerProjectId
        }, cancellationToken: cancellationToken);

    public async Task<List<DeveloperSearchResponse.Types.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default)
        => (await _service.DeveloperSearchAsync(new DeveloperSearchRequest
        {
            Term = term
        }, cancellationToken: cancellationToken)).Items.ToList();

    public async Task<int> GetAcvRealEstateType(int realEstateStateId, int realEstateSubtypeId, int realEstateTypeId, CancellationToken cancellationToken = default)
        => (await _service.GetAcvRealEstateTypeAsync(new GetAcvRealEstateTypeRequest
        {
            RealEstateTypeId = realEstateTypeId,
            RealEstateSubtypeId = realEstateSubtypeId,
            RealEstateStateId = realEstateTypeId
        }, cancellationToken: cancellationToken)).AcvRealEstateTypeId;

    private readonly ClientsMemoryCache _cache;
    private readonly Contracts.v1.CodebookService.CodebookServiceClient _service;

    public CodebookService(Contracts.v1.CodebookService.CodebookServiceClient service, ClientsMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }        
}
