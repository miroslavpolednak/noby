using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Clients.Interfaces;
public interface IMaintananceServiceClient
{
    Task ImportOfferFromDatamart(ImportOfferFromDatamartRequest request, CancellationToken cancellationToken = default);

    Task DeleteRefixationOfferOlderThan(DeleteRefixationOfferOlderThanRequest request, CancellationToken cancellationToken = default);
}
