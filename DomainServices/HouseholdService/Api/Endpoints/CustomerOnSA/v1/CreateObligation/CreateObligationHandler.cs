using CIS.Infrastructure.Caching.Grpc;
using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.v1.CreateObligation;

internal sealed class CreateObligationHandler(
    IDocumentDataStorage _documentDataStorage,
    ObligationMapper _mapper,
    ILogger<CreateObligationHandler> _logger)
        : IRequestHandler<CreateObligationRequest, CreateObligationResponse>
{
    public async Task<CreateObligationResponse> Handle(CreateObligationRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = _mapper.MapToData(request);

        var id = await _documentDataStorage.Add(request.CustomerOnSAId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.DocumentDataEntities.Obligation), id);

        return new()
        {
            ObligationId = id
        };
    }
}