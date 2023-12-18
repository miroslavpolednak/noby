using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferDetail;

internal sealed class GetMortgageOfferDetailHandler
    : IRequestHandler<GetMortgageOfferDetailRequest, GetMortgageOfferDetailResponse>
{
    public async Task<GetMortgageOfferDetailResponse> Handle(GetMortgageOfferDetailRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .Offers
            .AsNoTracking()
            .Where(t => t.OfferId == request.OfferId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        var offerData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.OfferData>(request.OfferId, cancellationToken);
        var mappedOfferData = _offerMapper.MapFromDataToSingle(offerData!.Data!.BasicParameters, offerData.Data.SimulationInputs, offerData.Data.SimulationOutputs);

        var additionalData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.AdditionalSimulationResultsData>(request.OfferId, cancellationToken);
        var mappedAdditionalData = _additionalResultsMapper.MapFromDataToSingle(additionalData!.Data);

        var worthinessData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.CreditWorthinessSimpleData>(request.OfferId, cancellationToken);
        var mappedWorthinessData = _creditWorthinessMapper.MapFromDataToSingle(worthinessData?.Data);

        return new GetMortgageOfferDetailResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new SharedTypes.GrpcTypes.ModificationStamp(entity),
            BasicParameters = mappedOfferData.BasicParameters,
            SimulationInputs = mappedOfferData.SimulationInputs,
            SimulationResults = mappedOfferData.SimulationResults,
            AdditionalSimulationResults = mappedAdditionalData,
            IsCreditWorthinessSimpleRequested = entity.IsCreditWorthinessSimpleRequested,
            CreditWorthinessSimpleInputs = mappedWorthinessData.Inputs
        };
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Database.DocumentDataEntities.Mappers.OfferDataMapper _offerMapper;
    private readonly Database.DocumentDataEntities.Mappers.AdditionalSimulationResultsDataMapper _additionalResultsMapper;
    private readonly Database.DocumentDataEntities.Mappers.CreditWorthinessSimpleDataMapper _creditWorthinessMapper;
    private readonly Database.OfferServiceDbContext _dbContext;

    public GetMortgageOfferDetailHandler(
        IDocumentDataStorage documentDataStorage,
        Database.DocumentDataEntities.Mappers.AdditionalSimulationResultsDataMapper additionalResultsMapper,
        Database.DocumentDataEntities.Mappers.CreditWorthinessSimpleDataMapper creditWorthinessMapper,
        Database.DocumentDataEntities.Mappers.OfferDataMapper offerMapper,
        Database.OfferServiceDbContext dbContext)
    {
        _additionalResultsMapper = additionalResultsMapper;
        _creditWorthinessMapper = creditWorthinessMapper;
        _documentDataStorage = documentDataStorage;
        _offerMapper = offerMapper;
        _dbContext = dbContext;
    }
}