using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1;

public interface IIdentifiedSubjectBrClient
    : IExternalServiceClient
{
    Task<IdentifiedSubjectResult<CreateIdentifiedSubjectResponse>> CreateIdentifiedSubject(IdentifiedSubject request, bool hardCreate, CancellationToken cancellationToken = default);

    Task UpdateIdentifiedSubject(long customerId, IdentifiedSubject request, CancellationToken cancellationToken = default);
    
    const string Version = "V1";
}