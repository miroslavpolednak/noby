namespace DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr.V1;

public interface IIdentifiedSubjectClient
{
    Task<CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(IdentifiedSubject request, bool hardCreate, string traceId, CancellationToken cancellationToken);
}