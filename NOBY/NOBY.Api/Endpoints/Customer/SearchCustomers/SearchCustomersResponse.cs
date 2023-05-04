namespace NOBY.Api.Endpoints.Customer.SearchCustomers;

public sealed class SearchCustomersResponse
{
    /// <summary>
    /// Informace o strankovani a razeni.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationResponse? Pagination { get; set; }
    
    /// <summary>
    /// Kolekce nalezenych klientu.
    /// </summary>
    public List<Dto.CustomerInList>? Rows { get; set; }
}