namespace FOMS.Api.Endpoints.Customer.Search.Dto;

public class CustomerInList
    : BaseCustomer
{
    public string? Street { get; set; }
    public string? Postcode { get; set; }
    public string? City { get; set; }
}