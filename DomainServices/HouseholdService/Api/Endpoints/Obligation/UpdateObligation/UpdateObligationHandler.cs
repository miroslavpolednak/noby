using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Obligation.UpdateObligation;

internal sealed class UpdateObligationHandler
    : IRequestHandler<Contracts.Obligation, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.Obligation request, CancellationToken cancellationToken)
    {
        var documentEntity = _mapper.MapToData(request);

        await _documentDataStorage.Update(request.ObligationId, documentEntity);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ObligationMapper _mapper;

    public UpdateObligationHandler(
        IDocumentDataStorage documentDataStorage,
        ObligationMapper mapper)
    {
        _documentDataStorage = documentDataStorage;
        _mapper = mapper;
    }
}