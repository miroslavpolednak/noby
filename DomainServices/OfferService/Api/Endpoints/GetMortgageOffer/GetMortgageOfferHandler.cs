using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOffer;

internal sealed class GetMortgageOfferHandler
    : IRequestHandler<GetMortgageOfferRequest, GetMortgageOfferResponse>
{
    public async Task<GetMortgageOfferResponse> Handle(GetMortgageOfferRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
           .Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .Select(t => new
           {
               t.ResourceProcessId,
               t.CreatedUserId,
               t.CreatedUserName,
               t.CreatedTime
           })
           .FirstOrDefaultAsync(cancellationToken) 
           ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        // json data
        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.OfferData>(request.OfferId, cancellationToken);
        var mappedOfferData = _offerMapper.MapFromDataToSingle(offerData!.Data!.BasicParameters, offerData.Data.SimulationInputs, offerData.Data.SimulationOutputs);

        var model = new GetMortgageOfferResponse
        {
            OfferId = request.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new SharedTypes.GrpcTypes.ModificationStamp(entity.CreatedUserId, entity.CreatedUserName, entity.CreatedTime),
            BasicParameters = mappedOfferData.BasicParameters,
            SimulationInputs = mappedOfferData.SimulationInputs,
            SimulationResults = mappedOfferData.SimulationResults
        };

        return model;
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Database.DocumentDataEntities.Mappers.OfferDataMapper _offerMapper;
    private readonly Database.OfferServiceDbContext _dbContext;

    public GetMortgageOfferHandler(
        Database.OfferServiceDbContext dbContext,
        IDocumentDataStorage documentDataStorage,
        Database.DocumentDataEntities.Mappers.OfferDataMapper offerMapper)
    {
        _documentDataStorage = documentDataStorage;
        _dbContext = dbContext;
        _offerMapper = offerMapper;
    }
}