using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.OfferService.Contracts;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;

namespace DomainServices.OfferService.Clients;

internal class OfferService : IOfferServiceClients
{

    #region Construction

    private readonly ILogger<OfferService> _logger;
    private readonly Contracts.v1.OfferService.OfferServiceClient _service;

    public OfferService(
        ILogger<OfferService> logger,
        Contracts.v1.OfferService.OfferServiceClient service)
    {
        _service = service;
        _logger = logger;
    }

    #endregion

    public async Task<IServiceCallResult> GetOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetOfferAsync(new OfferIdRequest() { OfferId = offerId }, cancellationToken: cancellationToken);

        return new SuccessfulServiceCallResult<GetOfferResponse>(result);
    }

    public async Task<IServiceCallResult> GetMortgageOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetMortgageOfferAsync(new OfferIdRequest() { OfferId = offerId }, cancellationToken: cancellationToken);

        return new SuccessfulServiceCallResult<GetMortgageOfferResponse>(result);
    }

    public async Task<IServiceCallResult> GetMortgageOfferDetail(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetMortgageOfferDetailAsync(new OfferIdRequest() { OfferId = offerId }, cancellationToken: cancellationToken);

        return new SuccessfulServiceCallResult<GetMortgageOfferDetailResponse>(result);
    }

    public async Task<IServiceCallResult> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            var result = await _service.SimulateMortgageAsync(request, cancellationToken: cancellationToken);

            _logger.LogDebug("Clients SimulateMortgage saved as #{id}", result.OfferId);

            return new SuccessfulServiceCallResult<SimulateMortgageResponse>(result);
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.InvalidArgument) // EAS chyba zadani
        {
            _logger.LogDebug("Clients SimulateMortgage failed gracefully with code {code}", ex.GetExceptionCodeFromTrailers());

            return new ErrorServiceCallResult(ex.GetErrorMessagesFromRpcExceptionWithIntKeys());
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.FailedPrecondition) // EAS vratilo standardni chybu
        {
            int code = ex.GetExceptionCodeFromTrailers();

            _logger.LogDebug("Clients SimulateMortgage failed gracefully with code {code}", code);
            _logger.LogDebug("EAS error codes: {code} || {text}", ex.GetValueFromTrailers("eassimerrorcode-bin"), ex.GetValueFromTrailers("eassimerrortext-bin"));

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

    public async Task<IServiceCallResult> GetMortgageOfferFPSchedule(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetMortgageOfferFPScheduleAsync(new OfferIdRequest() { OfferId = offerId }, cancellationToken: cancellationToken);

        return new SuccessfulServiceCallResult<GetMortgageOfferFPScheduleResponse>(result);
    }

}
