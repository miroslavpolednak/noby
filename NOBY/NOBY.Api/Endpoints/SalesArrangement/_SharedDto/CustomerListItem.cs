namespace NOBY.Api.Endpoints.SalesArrangement.SharedDto;

public class CustomerListItem
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int CustomerRoleId { get; set; }
    public string? BirthNumber { get; set; }
    public string? PlaceOfBirth { get; set; }   
    public int? MaritalStatusId { get; set; }
    public SharedTypes.Types.Address? MainAddress { get; set; }
    public SharedTypes.Types.Address? ContactAddress { get; set; }
    public NOBY.Dto.ContactsConfirmedDto? Contacts { get; set; }
    public List<SharedTypes.Types.CustomerIdentity>? Identities { get; set; }
}