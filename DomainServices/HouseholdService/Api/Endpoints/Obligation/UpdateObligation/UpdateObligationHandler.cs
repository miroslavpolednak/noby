using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Obligation.UpdateObligation;

internal sealed class UpdateObligationHandler(
    IDocumentDataStorage _documentDataStorage,
    ObligationMapper _mapper)
        : IRequestHandler<Contracts.Obligation, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.Obligation request, CancellationToken cancellationToken)
    {
        var documentEntity = _mapper.MapToData(request);

        await _documentDataStorage.Update(request.ObligationId, documentEntity);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}