namespace FOMS.Api.Endpoints.Household.Dto;

public class CustomerInHousehold
    : Customer
{
    /// <summary>
    /// Role klienta
    /// </summary>
    public int RoleId { get; set; }
}