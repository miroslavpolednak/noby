namespace FOMS.Api.Notifications.Requests;

internal sealed class CaseCustomerUpdatedRequest
    : INotification
{
    public long CaseId { get; set; }
    public DateTime? DateOfBirthNaturalPerson { get; set; }
    public string? FirstNameNaturalPerson { get; set; }
    public string? Name { get; set; }
    public CIS.Core.Types.CustomerIdentity? Customer { get; set; }
}
