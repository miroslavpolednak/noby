namespace DomainServices.CaseService.Api.Endpoints.LinkTaskToCase;

public class LinkTaskToCaseRequest : IRequest
{
    public int TaskId { get; set; }
}