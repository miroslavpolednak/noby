namespace DomainServices.CaseService.Api.SharedDto;

internal sealed class CaseStateChangeRequestId
{
    public int RequestId { get; set; }
    public long CaseId { get; set; }
    public DateTime CreatedTime { get; set; }
}
