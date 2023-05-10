using CIS.Infrastructure.CisMediatR.Rollback;
using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.CisMediatR;

internal sealed class RollbackBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IRollbackCapable
{
    private readonly ILogger<TRequest> _logger;
    private readonly IRollbackAction<TRequest> _action;

    public RollbackBehavior(IRollbackAction<TRequest> action, ILogger<TRequest> logger)
    {
        _logger = logger;
        _action= action;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.RollbackStarted<TRequest>(request, ex);

            // spustit rollback
            await _action.ExecuteRollback(ex, request, cancellationToken);

            _logger.RollbackFinished();

            if (_action.OverrideThrownException)
            {
                throw _action.OnOverrideException(ex);
            }
            else
            {
                // rethrow exception
                throw;
            }
        }
    }
}
