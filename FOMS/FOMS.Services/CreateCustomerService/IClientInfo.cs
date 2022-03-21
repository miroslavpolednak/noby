namespace FOMS.Services.CreateCustomer;

public interface IClientInfo
{
    string? FirstName { get; }
    string? LastName { get; }
    DateTime? DateOfBirth { get; }
    CIS.Foms.Types.CustomerIdentity? Identity { get; }
}
