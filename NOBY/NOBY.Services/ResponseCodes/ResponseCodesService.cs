using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;

namespace NOBY.Services.ResponseCodes;

[ScopedService, SelfService]
public sealed class ResponseCodesService
{
    public async Task<List<Dto.Refinancing.RefinancingResponseCode>> GetMortgageResponseCodes(long caseId, OfferTypes offerType, CancellationToken cancellationToken)
    {
        // uplne vsechny response kody
        var allCodes = await _offerService.GetResponseCodeList(caseId, cancellationToken);
        
        // dostupne response kody podle codebooku
        var availableCodes = (await _codebookService.ResponseCodeTypes(cancellationToken))
            .Where(t => t.IsValid && (t.IsAvailableForRefixation && offerType == OfferTypes.MortgageRefixation) || (t.IsAvailableForRetention && offerType == OfferTypes.MortgageRetention))
            .ToList();

        return allCodes
            .Where(t => availableCodes.Any(x => x.Id == t.ResponseCodeTypeId))
            .Select(t =>
            {
                var cb = availableCodes.First(x => x.Id == t.ResponseCodeTypeId);

                var item = new Dto.Refinancing.RefinancingResponseCode
                {
                    Id = t.ResponseCodeId,
                    CreatedTime = t.Created.DateTime,
                    CreatedBy = t.Created.UserName,
                    ResponseCodeTypeId = t.ResponseCodeTypeId
                };

                if (cb.DataType == DomainServices.CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.Date)
                {
                    item.DataDateTime = DateTime.Parse(t.Data, CultureInfo.InvariantCulture);
                }
                else if (cb.DataType == DomainServices.CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.BankCode)
                {
                    item.DataBankCode = t.Data;
                }
                else
                {
                    item.DataString = t.Data;
                }

                return item;
            })
            .ToList();
    }

    private readonly ICodebookServiceClient _codebookService;
    private readonly IOfferServiceClient _offerService;

    public ResponseCodesService(IOfferServiceClient offerService, ICodebookServiceClient codebookService)
    {
        _offerService = offerService;
        _codebookService = codebookService;
    }
}
