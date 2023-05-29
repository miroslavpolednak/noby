using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.CodebookService.Contracts.v1;
using Google.Protobuf;
using System.Runtime.CompilerServices;

namespace DomainServices.CodebookService.Api.Database;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class DatabaseAggregate
{
    public async Task<T> GetListWithParam<T>(object param, [CallerMemberName] string method = "")
        where T : class
    {
        var item = Sql[method];
        var connectionProvider = getConnectionProvider(item.Provider);
        return await connectionProvider.ExecuteDapperQueryAsync<T>(async c => await c.QueryFirstOrDefaultAsync<T>(item.Query, param));
    }

    public async Task<T> GetFirstOrDefault<T>(object param, [CallerMemberName] string method = "")
        where T : class
    {
        var item = Sql[method];
        var connectionProvider = getConnectionProvider(item.Provider);

        using var connection = connectionProvider.Create();
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<T>(item.Query, param);
    }

    public Task<GenericCodebookResponse> GetGenericItems([CallerMemberName] string method = "")
        => GetItems<GenericCodebookResponse, GenericCodebookResponse.Types.GenericCodebookItem>(method);

    public Task<TResponse> GetItems<TResponse, TItem>([CallerMemberName] string method = "")
        where TResponse : class, IMessage, Contracts.IItemsResponse<TItem>
        where TItem : class, IMessage
    {
        var item = Sql[method];
        var connectionProvider = getConnectionProvider(item.Provider);

        var response = CodebookMemoryCache.GetOrCreate(method.ToString(), () =>
        {
            using var connection = connectionProvider.Create();
            connection.Open();
            var items = connection.Query<TItem>(item.Query).AsList();

            var response = Activator.CreateInstance<TResponse>();
            response.Items.AddRange(items);
            return response;
        });
        return Task.FromResult(response);
    }

    public List<T> GetList<T>(string sqlQueryId, int suffix)
        => GetList<T>($"{sqlQueryId}{suffix}");

    public List<T> GetList<T>(string sqlQueryId)
    {
        var item = Sql[sqlQueryId];
        var connectionProvider = getConnectionProvider(item.Provider);
        return connectionProvider.ExecuteDapperRawSqlToList<T>(item.Query);
    }

    public List<dynamic> GetDynamicList(string sqlQueryId, int suffix)
        => GetDynamicList($"{sqlQueryId}{suffix}");

    public List<dynamic> GetDynamicList(string sqlQueryId)
    {
        var item = Sql[sqlQueryId];
        var connectionProvider = getConnectionProvider(item.Provider);
        return connectionProvider.ExecuteDapperQuery<List<dynamic>>(c => c.Query(item.Query).AsList());
    }
    
    private IConnectionProvider getConnectionProvider(SqlQueryCollection.DatabaseProviders provider) => provider switch
    {
        SqlQueryCollection.DatabaseProviders.KonsDb => Konsdb,
        SqlQueryCollection.DatabaseProviders.Xxd => Xxd,
        SqlQueryCollection.DatabaseProviders.XxdHf => Xxdhf,
        SqlQueryCollection.DatabaseProviders.Self => SelfDb,
        _ => throw new NotImplementedException()
    };

    public readonly Database.SqlQueryCollection Sql;
    public readonly IConnectionProvider SelfDb;
    public readonly IConnectionProvider<IKonsdbDapperConnectionProvider> Konsdb;
    public readonly IConnectionProvider<IXxdHfDapperConnectionProvider> Xxdhf;
    public readonly IConnectionProvider<IXxdDapperConnectionProvider> Xxd;

    public DatabaseAggregate(
        IConnectionProvider selfDb,
        Database.SqlQueryCollection sql,
        IConnectionProvider<IKonsdbDapperConnectionProvider> konsdb,
        IConnectionProvider<IXxdHfDapperConnectionProvider> xxdhf,
        IConnectionProvider<IXxdDapperConnectionProvider> xxd)
    {
        Sql = sql;
        SelfDb = selfDb;
        Konsdb = konsdb;
        Xxdhf = xxdhf;
        Xxd = xxd;
    }
}
