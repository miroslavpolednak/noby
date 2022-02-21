using System.Data;
using CIS.Core.Data;
using CIS.Infrastructure.Logging;

namespace CIS.Infrastructure.Data;

public abstract class DapperBaseRepository<TLogger>
{
    private readonly ILogger<TLogger> _logger;
    private readonly IConnectionProvider _connectionProvider;

    protected DapperBaseRepository(ILogger<TLogger> logger, IConnectionProvider connectionProvider)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    // use for buffered queries that return a type
    protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            return await _connectionProvider.ExecuteDapperQuery(getData, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.DapperQueryException(ex);
            throw;
        }
    }

    // use for buffered queries that do not return a type
    protected async Task WithConnection(Func<IDbConnection, Task> getData)
    {
        try
        {
            await _connectionProvider.ExecuteDapperQuery(getData);
        }
        catch (Exception ex)
        {
            _logger?.DapperQueryException(ex);
            throw;
        }
    }

    //use for non-buffered queries that return a type
    protected async Task<TResult> WithConnection<TRead, TResult>(Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            return await _connectionProvider.ExecuteDapperQuery<TRead, TResult>(getData, process, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.DapperQueryException(ex);
            throw;
        }
    }
}
