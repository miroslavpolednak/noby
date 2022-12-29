namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

internal sealed class MockIdentifiedSubjectBrClient 
    : IIdentifiedSubjectBrClient
{
    public Task<Contracts.CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(Contracts.IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }
}