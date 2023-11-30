using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Obligation.GetObligation;

internal sealed class GetObligationHandler
    : IRequestHandler<GetObligationRequest, Contracts.Obligation>
{
    public async Task<Contracts.Obligation> Handle(GetObligationRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = await _documentDataStorage.FirstOrDefault<Database.DocumentDataEntities.Obligation>(request.ObligationId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ObligationNotFound, request.ObligationId);

        var model = _mapper.MapFromDataToSingle(documentEntity.Data!);

        model.ObligationId = documentEntity.DocumentDataStorageId;
        model.CustomerOnSAId = documentEntity.EntityIdInt;

        return model;
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ObligationMapper _mapper;

    public GetObligationHandler(IDocumentDataStorage documentDataStorage, ObligationMapper mapper)
    {
        _documentDataStorage = documentDataStorage;
        _mapper = mapper;
    }
}