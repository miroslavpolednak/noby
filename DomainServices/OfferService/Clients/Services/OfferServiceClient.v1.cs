﻿using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Clients.v1;

internal sealed class OfferServiceClient
    : IOfferServiceClient
{
    public async Task<decimal> GetInterestRate(long caseId, DateOnly futureInterestRateValidTo, int? fixedRatePeriod = default, CancellationToken cancellationToken = default)
        => (await _service.GetInterestRateAsync(new GetInterestRateRequest
        {
            CaseId = caseId,
            FutureInterestRateValidTo = futureInterestRateValidTo,
            FixedRatePeriod = fixedRatePeriod,
        }, cancellationToken: cancellationToken)).LoanInterestRate;

    public async Task<List<Contracts.GetOfferListResponse.Types.GetOfferListItem>> GetOfferList(long caseId, OfferTypes offerType, bool ommitParametersFromResponse = false, bool includeValidOnly = false, CancellationToken cancellationToken = default)
        => (await _service.GetOfferListAsync(new GetOfferListRequest
        {
            CaseId = caseId,
            OfferType = offerType,
            OmmitParametersFromResponse = ommitParametersFromResponse,
            IncludeValidOnly = includeValidOnly,
        }, cancellationToken: cancellationToken))
        .Offers
        .ToList();

    public async Task<ValidateOfferIdResponse> ValidateOfferId(int offerId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default)
        => await _service.ValidateOfferIdAsync(new ValidateOfferIdRequest
        {
            OfferId = offerId,
            ThrowExceptionIfNotFound = throwExceptionIfNotFound
        }, cancellationToken: cancellationToken);

    public async Task<GetOfferResponse> GetOffer(int offerId, CancellationToken cancellationToken = default)
    {
        if (_cacheGetMortgageOfferResponse is null || offerId != _cacheGetOfferResponseId)
        {
            _cacheGetMortgageOfferResponse = await _service.GetOfferAsync(new GetOfferRequest()
            {
                OfferId = offerId
            }, cancellationToken: cancellationToken);
            _cacheGetOfferResponseId = offerId;
        }
        return _cacheGetMortgageOfferResponse;
    }

    public async Task<GetMortgageDetailResponse> GetMortgageDetail(int offerId, CancellationToken cancellationToken = default)
        => await _service.GetMortgageDetailAsync(new GetMortgageDetailRequest()
        {
            OfferId = offerId
        }, cancellationToken: cancellationToken);

    public async Task<SimulateBuildingSavingsResponse> SimulateBuildingSavings(SimulateBuildingSavingsRequest request, CancellationToken cancellationToken = default)
        => await _service.SimulateBuildingSavingsAsync(request, cancellationToken: cancellationToken);

    public async Task<SimulateMortgageResponse> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default)
        => await _service.SimulateMortgageAsync(request, cancellationToken: cancellationToken);

    public async Task<SimulateMortgageRetentionResponse> SimulateMortgageRetention(SimulateMortgageRetentionRequest request, CancellationToken cancellationToken = default)
        => await _service.SimulateMortgageRetentionAsync(request, cancellationToken: cancellationToken);

    public async Task<GetMortgageOfferFPScheduleResponse> GetMortgageOfferFPSchedule(int offerId, CancellationToken cancellationToken = default)
        => await _service.GetMortgageOfferFPScheduleAsync(new GetMortgageOfferFPScheduleRequest()
        {
            OfferId = offerId
        }, cancellationToken: cancellationToken);

    public async Task<GetOfferDeveloperResponse> GetOfferDeveloper(int offerId, CancellationToken cancellationToken = default)
        => await _service.GetOfferDeveloperAsync(new GetOfferDeveloperRequest
        {
            OfferId = offerId
        }, cancellationToken: cancellationToken);

    public async Task UpdateOffer(UpdateOfferRequest request, CancellationToken cancellationToken = default)
        => await _service.UpdateOfferAsync(request, cancellationToken: cancellationToken);

    public async Task<SimulateMortgageRefixationResponse> SimulateMortgageRefixation(SimulateMortgageRefixationRequest request, CancellationToken cancellationToken = default)
        => await _service.SimulateMortgageRefixationAsync(request, cancellationToken: cancellationToken);

    public async Task<SimulateMortgageExtraPaymentResponse> SimulateMortgageExtraPayment(SimulateMortgageExtraPaymentRequest request, CancellationToken cancellationToken = default)
        => await _service.SimulateMortgageExtraPaymentAsync(request, cancellationToken: cancellationToken);

    public async Task<int> CreateResponseCode(CreateResponseCodeRequest request, CancellationToken cancellationToken = default)
        => (await _service.CreateResponseCodeAsync(request, cancellationToken: cancellationToken)).ResponseCodeId;

    public async Task<List<Contracts.GetResponseCodeListResponse.Types.GetResponseCodeItem>> GetResponseCodeList(long caseId, CancellationToken cancellationToken = default)
        => (await _service.GetResponseCodeListAsync(new GetResponseCodeListRequest
        {
            CaseId = caseId
        }, cancellationToken: cancellationToken))
        .Responses
        .ToList();

    public async Task DeleteOfferList(IEnumerable<int> offerIds, CancellationToken cancellationToken = default)
        => await _service.DeleteOfferListAsync(new DeleteOfferListRequest { OfferIds = { offerIds } }, cancellationToken: cancellationToken);

    public async Task CancelOffer(int offerId, CancellationToken cancellationToken = default)
        => await _service.CancelOfferAsync(new() { OfferId = offerId }, cancellationToken: cancellationToken);

    private int? _cacheGetOfferResponseId;
    private GetOfferResponse? _cacheGetMortgageOfferResponse;

    private readonly Contracts.v1.OfferService.OfferServiceClient _service;

    public OfferServiceClient(Contracts.v1.OfferService.OfferServiceClient service)
        => _service = service;
}
