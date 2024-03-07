﻿using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Clients.Services;

internal sealed class OfferService 
    : IOfferServiceClient
{
    public async Task<ValidateOfferIdResponse> ValidateOfferId(int offerId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default)
        => await _service.ValidateOfferIdAsync(new ValidateOfferIdRequest() 
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

    public async Task<GetOfferDetailResponse> GetOfferDetail(int offerId, CancellationToken cancellationToken = default)
        => await _service.GetOfferDetailAsync(new GetOfferDetailRequest() 
        { 
            OfferId = offerId 
        }, cancellationToken: cancellationToken);

    public async Task<SimulateMortgageResponse> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.SimulateMortgageAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<SimulateMortgageRetentionResponse> SimulateMortgageRetention(SimulateMortgageRetentionRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.SimulateMortgageRetentionAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetMortgageOfferFPScheduleResponse> GetMortgageOfferFPSchedule(int offerId, CancellationToken cancellationToken = default)
    {
        return await _service.GetMortgageOfferFPScheduleAsync(new GetMortgageOfferFPScheduleRequest()
        {
            OfferId = offerId
        }, cancellationToken: cancellationToken);
    }

    public async Task<GetOfferDeveloperResponse> GetOfferDeveloper(int offerId, CancellationToken cancellationToken = default) 
    {
        return await _service.GetOfferDeveloperAsync(new GetOfferDeveloperRequest
        {
            OfferId = offerId
        }, cancellationToken: cancellationToken);
    }

    public async Task UpdateOffer(UpdateOfferRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateOfferAsync(request, cancellationToken: cancellationToken);
    }

    private int? _cacheGetOfferResponseId;
    private GetOfferResponse? _cacheGetMortgageOfferResponse;

    private readonly Contracts.v1.OfferService.OfferServiceClient _service;

    public OfferService(Contracts.v1.OfferService.OfferServiceClient service)
        => _service = service;
}
