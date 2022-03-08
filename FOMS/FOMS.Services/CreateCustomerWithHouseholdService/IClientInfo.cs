namespace FOMS.Services.CreateCustomerWithHousehold;

public interface IClientInfo
{
    string? FirstName { get; }
    string? LastName { get; }
    DateTime? DateOfBirth { get; }
    CIS.Foms.Types.CustomerIdentity? Customer { get; }
}
