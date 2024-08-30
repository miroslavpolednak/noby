using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.GetObligation;

internal sealed class GetObligationHandler(IDocumentDataStorage _documentDataStorage, ObligationMapper _mapper)
        : IRequestHandler<GetObligationRequest, Contracts.Obligation>
{
    public async Task<Contracts.Obligation> Handle(GetObligationRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = await _documentDataStorage.FirstOrDefault<Database.DocumentDataEntities.Obligation, int>(request.ObligationId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.ObligationNotFound, request.ObligationId);

        var model = _mapper.MapFromDataToSingle(documentEntity.Data!)!;

        model.ObligationId = documentEntity.DocumentDataStorageId;
        model.CustomerOnSAId = documentEntity.EntityId;

        return model;
    }
}