using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CIS.Infrastructure.Logging;
using CIS.Infrastructure.CisMediatR.PayloadLogger;

namespace CIS.Infrastructure.CisMediatR;

public sealed class PayloadLoggerBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PayloadLoggerBehavior<TRequest, TResponse>> _logger;
    private JsonConvertByteString _byteStringConverter = new JsonConvertByteString();

    public PayloadLoggerBehavior(ILogger<PayloadLoggerBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", JsonConvert.SerializeObject(request, _byteStringConverter) }
            }))
        {
            _logger.RequestHandlerStarted(requestName);
        }
        
        var response = await next();

        if (response is null || response is Unit)
        {
            _logger.RequestHandlerFinishedWithEmptyResult(requestName);
        }
        else
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
