namespace FOMS.Api.Endpoints.Household.Dto;

public class CustomerInHousehold
    : Customer
{
    /// <summary>
    /// Role klienta
    /// </summary>
    /// <example>1</example>
    public int RoleId { get; set; }
}