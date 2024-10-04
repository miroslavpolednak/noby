using DomainServices.OfferService.Api.Database;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetOfferList;

internal sealed class GetOfferListHandler(
    OfferServiceDbContext _dbContext,
    TimeProvider _dateTime,
    IDocumentDataStorage _documentDataStorage,
    Database.DocumentDataEntities.Mappers.MortgageOfferDataMapper _offerMapper,
    Database.DocumentDataEntities.Mappers.MortgageRetentionDataMapper _retentionMapper,
    Database.DocumentDataEntities.Mappers.MortgageRefixationDataMapper _refixationMapper,
    Database.DocumentDataEntities.Mappers.MortgageExtraPaymentDataMapper _extraPaymentMapper)
        : IRequestHandler<GetOfferListRequest, GetOfferListResponse>
{
    public async Task<GetOfferListResponse> Handle(GetOfferListRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Offers
            .AsNoTracking()
            .Where(t => t.CaseId == request.CaseId && t.OfferType == (int)request.OfferType);
        
        if (request.IncludeValidOnly)
        {
            query = query.Where(o => o.ValidTo >= _dateTime.GetLocalNow().Date);
        }
        
        var list = await query.Select(DatabaseExpressions.CreateCommonOfferData()).ToListAsync(cancellationToken);

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
                        result.MortgageExtraPayment = _extraPaymentMapper.MapToFullData((Database.DocumentDataEntities.MortgageExtraPaymentData)offersData[offer.OfferId]);
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
            OfferTypes.Mortgage => (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageOfferData, int>(ids, cancellationToken)).ToDictionary(k => k.EntityId, v => (IDocumentData)v.Data!),
            OfferTypes.MortgageRetention => (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageRetentionData, int>(ids, cancellationToken)).ToDictionary(k => k.EntityId, v => (IDocumentData)v.Data!),
            OfferTypes.MortgageRefixation => (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageRefixationData, int>(ids, cancellationToken)).ToDictionary(k => k.EntityId, v => (IDocumentData)v.Data!),
            OfferTypes.MortgageExtraPayment => (await _documentDataStorage.GetList<Database.DocumentDataEntities.MortgageExtraPaymentData, int>(ids, cancellationToken)).ToDictionary(k => k.EntityId, v => (IDocumentData)v.Data!),
            _ => throw new NotImplementedException()
        };
    }
}
