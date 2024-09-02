using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CIS.Infrastructure.Logging;
using CIS.Infrastructure.CisMediatR.PayloadLogger;

namespace CIS.Infrastructure.CisMediatR;

public sealed class PayloadLoggerBehavior<TRequest, TResponse>(
    ILogger<PayloadLoggerBehavior<TRequest, TResponse>> _logger, 
    PayloadLoggerBehaviorConfiguration? _configuration)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly JsonConvertByteString _byteStringConverter = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        
        if (_configuration?.LogRequestPayload ?? false)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", JsonConvert.SerializeObject(request, _byteStringConverter) }
            }))
            {
                _logger.RequestHandlerStarted(requestName);
            }
        }
        
        var response = await next();

        if (response is null || response is Unit)
        {
            _logger.RequestHandlerFinishedWithEmptyResult(requestName);
        }
        else if (_configuration?.LogResponsePayload ?? false)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", JsonConvert.SerializeObject(response, _byteStringConverter) }
            }))
            {
                _logger.RequestHandlerFinished(requestName);
            }
        }
        
        return response;
    }
}
