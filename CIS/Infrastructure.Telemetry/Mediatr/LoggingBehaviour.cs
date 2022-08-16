using Microsoft.Extensions.Logging;
using MediatR;

namespace CIS.Infrastructure.Telemetry.Mediatr;

internal class LoggingBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        string requestName = typeof(TRequest).Name;

        using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", System.Text.Json.JsonSerializer.Serialize(request) }
            }))
        {
            _logger.RequestHandlerStarted(requestName);
        }
        
        var response = await next();

        if (response is null || response is MediatR.Unit)
            _logger.RequestHandlerFinished(requestName);
        else
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", System.Text.Json.JsonSerializer.Serialize(response) }
            }))
            {
                _logger.RequestHandlerFinished(requestName);
            }
        }
        
        return response;
    }
}
