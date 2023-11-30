using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Obligation.CreateObligation;

internal sealed class CreateObligationHandler
    : IRequestHandler<CreateObligationRequest, CreateObligationResponse>
{
    public async Task<CreateObligationResponse> Handle(CreateObligationRequest request, CancellationToken cancellationToken)
    {
        var documentEntity = _mapper.MapToData(request);

        var id = await _documentDataStorage.Add(request.CustomerOnSAId, documentEntity, cancellationToken);

        _logger.EntityCreated(nameof(Database.DocumentDataEntities.Obligation), id);

        return new CreateObligationResponse
        {
            ObligationId = id
        };
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ObligationMapper _mapper;
    private readonly ILogger<CreateObligationHandler> _logger;

    public CreateObligationHandler(
        IDocumentDataStorage documentDataStorage,
        ObligationMapper mapper,
        ILogger<CreateObligationHandler> logger)
    {
        _documentDataStorage = documentDataStorage;
        _mapper = mapper;
        _logger = logger;
    }
}