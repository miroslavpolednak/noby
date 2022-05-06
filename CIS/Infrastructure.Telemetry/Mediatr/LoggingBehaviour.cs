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

        _logger.RequestHandlerStarted(requestName, request);

        var response = await next();

        _logger.RequestHandlerFinished(requestName, response);
        
        return response;
    }
}
