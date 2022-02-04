using CIS.Core.Results;
using CIS.Infrastructure.gRPC;
using DomainServices.OfferService.Contracts;
using Grpc.Core;
using Microsoft.Extensions.Logging;

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

    public async Task<IServiceCallResult> SimulateBuildingSavings(SimulateBuildingSavingsRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction SimulateBuildingSavings call with {request}", request);

        try
        {
            var result = await _userContext.AddUserContext(async () => await _service.SimulateBuildingSavingsAsync(request, cancellationToken: cancellationToken));

            _logger.LogDebug("Abstraction SimulateBuildingSavings saved as #{id}", result.OfferInstanceId);

            return new SuccessfulServiceCallResult<SimulateBuildingSavingsResponse>(result);
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.InvalidArgument) // EAS chyba zadani
        {
            _logger.LogDebug("Abstraction SimulateBuildingSavings failed gracefully with code {code}", ex.GetExceptionCodeFromTrailers());
            
            return new ErrorServiceCallResult(ex.GetErrorMessagesFromRpcExceptionWithIntKeys());
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.FailedPrecondition) // EAS vratilo standardni chybu
        {
            int code = ex.GetExceptionCodeFromTrailers();

            _logger.LogDebug("Abstraction SimulateBuildingSavings failed gracefully with code {code}", code);
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

    public async Task<IServiceCallResult> GetBuildingSavingsData(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction GetBuildingSavingsData call with #ID {offerInstanceId}", offerInstanceId);

        var result = await _userContext.AddUserContext(async () => await _service.GetBuildingSavingsDataAsync(new OfferInstanceIdRequest() { OfferInstanceId = offerInstanceId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetBuildingSavingsDataResponse>(result);
    }

    public async Task<IServiceCallResult> GetBuildingSavingsDepositSchedule(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction GetBuildingSavingsDepositSchedule call with #ID {offerInstanceId}", offerInstanceId);

        var result = await _userContext.AddUserContext(async () => await _service.GetBuildingSavingsDepositScheduleAsync(new OfferInstanceIdRequest() { OfferInstanceId = offerInstanceId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetBuildingSavingsScheduleResponse>(result);
    }

    public async Task<IServiceCallResult> GetBuildingSavingsPaymentSchedule(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction GetBuildingSavingsPaymentSchedule call with #ID {offerInstanceId}", offerInstanceId);

        var result = await _userContext.AddUserContext(async () => await _service.GetBuildingSavingsPaymentScheduleAsync(new OfferInstanceIdRequest() { OfferInstanceId = offerInstanceId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetBuildingSavingsScheduleResponse>(result);
    }

    public async Task<IServiceCallResult> PrintBuildingSavingsOffer(PrintBuildingSavingsOfferRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction PrintBuildingSavingsOffer call with #ID {offerInstanceId}", request.OfferInstanceId);

        var result = await _userContext.AddUserContext(async () => await _service.PrintBuildingSavingsOfferAsync(request, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<PrintBuildingSavingsOfferResponse>(result);
    }

    public async Task<IServiceCallResult> SimulateMortgage(SimulateMortgageRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction SimulateMortgage call with {request}", request);

        try
        {
            var result = await _userContext.AddUserContext(async () => await _service.SimulateMortgageAsync(request, cancellationToken: cancellationToken));

            _logger.LogDebug("Abstraction SimulateMortgage saved as #{id}", result.OfferInstanceId);

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

    public async Task<IServiceCallResult> GetMortgageData(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction GetMortgageData call with #ID {offerInstanceId}", offerInstanceId);

        var result = await _userContext.AddUserContext(async () => await _service.GetMortgageDataAsync(new OfferInstanceIdRequest() { OfferInstanceId = offerInstanceId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetMortgageDataResponse>(result);
    }

    public async Task<IServiceCallResult> GetOfferInstance(int offerInstanceId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogDebug("Abstraction GetOfferInstance call with #ID {offerInstanceId}", offerInstanceId);

        var result = await _userContext.AddUserContext(async () => await _service.GetOfferInstanceAsync(new OfferInstanceIdRequest() { OfferInstanceId = offerInstanceId }, cancellationToken: cancellationToken));

        return new SuccessfulServiceCallResult<GetOfferInstanceResponse>(result);
    }

}
