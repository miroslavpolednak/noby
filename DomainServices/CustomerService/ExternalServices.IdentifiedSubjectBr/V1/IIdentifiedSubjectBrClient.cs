using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

public interface IIdentifiedSubjectBrClient
    : IExternalServiceClient
{
    Task<Contracts.CreateIdentifiedSubjectResponse> CreateIdentifiedSubject(Contracts.IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default);
    Task UpdateIdentifiedSubject(long customerId, Contracts.IdentifiedSubject request, CancellationToken cancellationToken = default);
    
    const string Version = "V1";
}