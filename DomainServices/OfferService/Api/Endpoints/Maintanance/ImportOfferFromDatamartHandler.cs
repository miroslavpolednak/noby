using DomainServices.MaintananceService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.OfferService.Api.Endpoints.Maintanance;

public class ImportOfferFromDatamartHandler : IRequestHandler<ImportOfferFromDatamartRequest, Empty>
{
    public Task<Empty> Handle(ImportOfferFromDatamartRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
