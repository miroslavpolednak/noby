namespace NOBY.Api.Endpoints.Customer.SearchCustomers;

public sealed class SearchCustomersRequest
    : IRequest<SearchCustomersResponse>
{
    /// <summary>
    /// Podle ceho se ma hledat
    /// </summary>
    public Dto.SearchData? SearchData { get; set; }
    
    /// <summary>
    /// Nastaveni strankovani a razeni kolekce Cases.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationRequest? Pagination { get; set; }
}