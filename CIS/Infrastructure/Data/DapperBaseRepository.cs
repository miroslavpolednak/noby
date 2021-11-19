using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;
using CIS.Core.Data;

namespace CIS.Infrastructure.Data
{
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
        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    return await getData(connection);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }

        // use for buffered queries that do not return a type
        protected async Task WithConnection(Func<IDbConnection, Task> getData)
        {
            try
            {
                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    await getData(connection);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }

        //use for non-buffered queries that return a type
        protected async Task<TResult> WithConnection<TRead, TResult>(Func<IDbConnection, Task<TRead>> getData, Func<TRead, Task<TResult>> process)
        {
            try
            {
                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    var data = await getData(connection);
                    return await process(data);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
