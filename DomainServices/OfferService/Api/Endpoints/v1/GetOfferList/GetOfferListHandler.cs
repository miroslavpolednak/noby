using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetOfferList;

internal sealed class GetOfferListHandler
    : IRequestHandler<GetOfferListRequest, GetOfferListResponse>
{
    public async Task<GetOfferListResponse> Handle(GetOfferListRequest request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.Offers
            .AsNoTracking()
            .Where(t => t.CaseId == request.CaseId && t.OfferType == (int)request.OfferType)
            .Select(DatabaseExpressions.CreateCommonOfferData())
            .ToListAsync(cancellationToken);

        // get json data
        int[] offerIds = list.Select(t => t.OfferId).ToArray();
        var offersData = await getData(request.OfferType, offerIds, cancellationToken);

        GetOfferListResponse response = new();
        response.Offers.AddRange(list.Select(offer =>
        {
            var result = new GetOfferListResponse.Types.GetOfferListItem
            {
                Data = offer
            };

            if (!request.OmmitParametersFromResponse)
            {
                switch (offer.OfferType)
                {
                    case OfferTypes.Mortgage:
                        result.MortgageOffer = _offerMapper.MapToFullData((Database.DocumentDataEntities.MortgageOfferData)offersData[offer.OfferId]);
                        break;

                    case OfferTypes.MortgageRetention:
                        result.MortgageRetention = _retentionMapper.MapToFullData((Database.DocumentDataEntities.MortgageRetentionData)offersData[offer.OfferId]);
                        break;

                    case OfferTypes.MortgageRefixation:
                        result.MortgageRefixation = _refixationMapper.MapToFullData((Database.DocumentDataEntities.MortgageRefixationData)offersData[offer.OfferId]);
                        break;

                    case OfferTypes.MortgageExtraPayment:
                        //result.MortgageExtraPayment = _offerMapper.MapToFullData((Database.DocumentDataEntities.MortgageOfferData)offersData[offer.OfferId]);
                        break;
                }
            }

            return result;
        }));

        return response;
    }

    private async Task<Dictionary<int, IDocumentData>> getData(OfferTypes offerType, int[] ids, CancellationToken cancellationToken)
    {
        return offerType switch
        {
            OfferTypes.Mortgage => (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageOfferData>(ids, cancellationToken)).ToDictionary(k => k.EntityIdInt, v => (IDocumentData)v.Data!),
            OfferTypes.MortgageRetention => (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageRetentionData>(ids, cancellationToken)).ToDictionary(k => k.EntityIdInt, v => (IDocumentData)v.Data!),
            OfferTypes.MortgageRefixation => (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageRefixationData>(ids, cancellationToken)).ToDictionary(k => k.EntityIdInt, v => (IDocumentData)v.Data!),
            OfferTypes.MortgageExtraPayment => (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageExtraPaymentData>(ids, cancellationToken)).ToDictionary(k => k.EntityIdInt, v => (IDocumentData)v.Data!),
            _ => throw new NotImplementedException()
        };
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly OfferServiceDbContext _dbContext;
    private readonly Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper _offerMapper;
    private readonly Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper _retentionMapper;
    private readonly Database.DocumentDataEntities.Mappers.MortgageRefixationDataMapper _refixationMapper;

    public GetOfferListHandler(OfferServiceDbContext dbContext, IDocumentDataStorage documentDataStorage, Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper offerMapper, Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper retentionMapper, Database.DocumentDataEntities.Mappers.MortgageRefixationDataMapper refixationMapper)
    {
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
        _offerMapper = offerMapper;
        _retentionMapper = retentionMapper;
        _refixationMapper = refixationMapper;
    }
}
