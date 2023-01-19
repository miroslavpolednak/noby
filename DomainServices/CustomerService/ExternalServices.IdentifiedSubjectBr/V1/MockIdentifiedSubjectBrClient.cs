using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

internal sealed class MockIdentifiedSubjectBrClient : IIdentifiedSubjectBrClient
{
    public Task<CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateIdentifiedSubject(long customerId, IdentifiedSubject request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}