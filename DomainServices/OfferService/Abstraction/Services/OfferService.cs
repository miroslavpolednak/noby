using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.OfferService.Contracts;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;

namespace DomainServices.OfferService.Abstraction;

internal class OfferService : IOfferServiceAbstraction
{

    #region Construction

    private readonly ILogger<OfferService> _logger;
    private readonly Contracts.v1.OfferService.OfferServiceClient _service;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;

    public OfferService(
        ILogger<OfferService> logger,
        Contracts.v1.OfferService.OfferServiceClient service,
        CIS.Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }

    #endregion

    public async Task<IServiceCallResult> GetOffer(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetOffer), offerId);

        var result = await _userContext.AddUserContext(async () => await _service.GetOfferAsync(new OfferIdRequest() { OfferId = offerId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetOfferResponse>(result);
    }

    public async Task<IServiceCallResult> GetMortgageData(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetMortgageData), offerId);

        var result = await _userContext.AddUserContext(async () => await _service.GetMortgageDataAsync(new OfferIdRequest() { OfferId = offerId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetMortgageDataResponse>(result);
    }

    public async Task<IServiceCallResult> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(SimulateMortgage));

        try
        {
            var result = await _userContext.AddUserContext(async () => await _service.SimulateMortgageAsync(request, cancellationToken: cancellationToken));

            _logger.LogDebug("Abstraction SimulateMortgage saved as #{id}", result.OfferId);

            return new SuccessfulServiceCallResult<SimulateMortgageResponse>(result);
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.InvalidArgument) // EAS chyba zadani
        {
            _logger.LogDebug("Abstraction SimulateMortgage failed gracefully with code {code}", ex.GetExceptionCodeFromTrailers());

            return new ErrorServiceCallResult(ex.GetErrorMessagesFromRpcExceptionWithIntKeys());
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.FailedPrecondition) // EAS vratilo standardni chybu
        {
            int code = ex.GetExceptionCodeFromTrailers();

            _logger.LogDebug("Abstraction SimulateMortgage failed gracefully with code {code}", code);
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

}
