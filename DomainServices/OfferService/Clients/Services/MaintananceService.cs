using DomainServices.OfferService.Clients.Interfaces;
using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Clients.Services;
public class MaintananceService(Contracts.MaintananceService.MaintananceServiceClient service) : IMaintananceService
{
    private readonly Contracts.MaintananceService.MaintananceServiceClient _service = service;

    public async Task ImportOfferFromDatamart(ImportOfferFromDatamartRequest request, CancellationToken cancellationToken = default)
     => await _service.ImportOfferFromDatamartAsync(request, cancellationToken: cancellationToken);
}
