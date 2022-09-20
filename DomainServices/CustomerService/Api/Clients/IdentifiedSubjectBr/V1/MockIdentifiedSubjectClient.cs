namespace DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr.V1;

internal class MockIdentifiedSubjectClient : IIdentifiedSubjectClient
{
    public Task<CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(IdentifiedSubject request, bool hardCreate, string traceId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}