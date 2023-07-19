using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Clients.Services;

public abstract partial class CodebookServiceBaseMock
{
    public virtual Task<List<DeveloperSearchResponse.Types.DeveloperSearchItem>> DeveloperSearch(string term, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
    
    public virtual Task<GetDeveloperResponse> GetDeveloper(int developerId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public virtual Task<GetDeveloperProjectResponse> GetDeveloperProject(int developerId, int developerProjectId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public virtual Task<GetOperatorResponse> GetOperator(string performerLogin, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public virtual Task<string> GetAcvRealEstateType(int realEstateStateId, int realEstateSubtypeId, int realEstateTypeId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
