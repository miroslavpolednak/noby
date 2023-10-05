namespace NOBY.Api.Endpoints.Customer.SearchCustomers.Dto;

public class CustomerInList
    : BaseCustomer
{
    public SharedTypes.Types.CustomerIdentity? Identity { get; set; }
    
    public string? Street { get; set; }
    
    public string? Postcode { get; set; }
    
    public string? City { get; set; }
}