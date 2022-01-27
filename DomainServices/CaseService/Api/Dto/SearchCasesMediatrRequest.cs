namespace DomainServices.CaseService.Api.Dto;

internal sealed class SearchCasesMediatrRequest
    : IRequest<Contracts.SearchCasesResponse>, CIS.Core.Validation.IValidatableRequest
{
    public Contracts.SearchCasesRequest Request { get; init; }
    
    public SearchCasesMediatrRequest(Contracts.SearchCasesRequest request)
    {
        Request = request;
    }
}
