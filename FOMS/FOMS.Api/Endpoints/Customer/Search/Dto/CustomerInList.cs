namespace FOMS.Api.Endpoints.Customer.Search.Dto;

public class CustomerInList
    : BaseCustomer
{
    public CIS.Foms.Types.CustomerIdentity? Identity { get; set; }
    
    public string? Street { get; set; }
    
    public string? Postcode { get; set; }
    
    public string? City { get; set; }
}