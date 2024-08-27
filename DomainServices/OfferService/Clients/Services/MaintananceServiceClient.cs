using DomainServices.OfferService.Clients.Interfaces;
using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Clients.Services;
public class MaintananceServiceClient(Contracts.MaintananceService.MaintananceServiceClient service) : IMaintananceServiceClient
{
    private readonly Contracts.MaintananceService.MaintananceServiceClient _service = service;

    public async Task DeleteRefixationOfferOlderThan(DeleteRefixationOfferOlderThanRequest request, CancellationToken cancellationToken = default)
     => await _service.DeleteRefixationOfferOlderThanAsync(request, cancellationToken: cancellationToken);

    public async Task ImportOfferFromDatamart(ImportOfferFromDatamartRequest request, CancellationToken cancellationToken = default)
     => await _service.ImportOfferFromDatamartAsync(request, cancellationToken: cancellationToken);
}
