﻿using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.OfferService.Contracts;
using Grpc.Core;

namespace DomainServices.OfferService.Clients.Services;

internal sealed class OfferService 
    : IOfferServiceClient
{
    public async Task<GetOfferResponse> GetOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken))
        => await _service.GetOfferAsync(new GetOfferRequest() 
        { 
            OfferId = offerId 
        }, cancellationToken: cancellationToken);

    public async Task<GetMortgageOfferResponse> GetMortgageOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken))
        => await _service.GetMortgageOfferAsync(new GetMortgageOfferRequest() 
        { 
            OfferId = offerId 
        }, cancellationToken: cancellationToken);

    public async Task<GetMortgageOfferDetailResponse> GetMortgageOfferDetail(int offerId, CancellationToken cancellationToken = default(CancellationToken))
        => await _service.GetMortgageOfferDetailAsync(new GetMortgageOfferDetailRequest() 
        { 
            OfferId = offerId 
        }, cancellationToken: cancellationToken);

    public async Task<SimulateMortgageResponse> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            return await _service.SimulateMortgageAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.InvalidArgument) // EAS chyba zadani
        {
            return new ErrorServiceCallResult(ex.GetErrorMessagesFromRpcExceptionWithIntKeys());
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.FailedPrecondition) // EAS vratilo standardni chybu
        {
            int code = ex.GetExceptionCodeFromTrailers();

            return code switch
            {
                10011 => new SimulationServiceErrorResult(ex.GetValueFromTrailers("eassimerrorcode-bin"), ex.GetValueFromTrailers("eassimerrortext-bin")),
                _ => new ErrorServiceCallResult(ex.GetErrorMessagesFromRpcExceptionWithIntKeys())
            };
        }
        catch (Exception err)
        {
            var x = err;
            throw;
        }
    }

    public async Task<GetMortgageOfferFPScheduleResponse> GetMortgageOfferFPSchedule(int offerId, CancellationToken cancellationToken = default(CancellationToken))
        => await _service.GetMortgageOfferFPScheduleAsync(new GetMortgageOfferFPScheduleRequest() 
        { 
            OfferId = offerId 
        }, cancellationToken: cancellationToken);

    private readonly Contracts.v1.OfferService.OfferServiceClient _service;
    public OfferService(Contracts.v1.OfferService.OfferServiceClient service)
        => _service = service;
}
