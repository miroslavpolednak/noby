using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Data;
using DomainServices.CodebookService.Api.Database;

namespace DomainServices.CodebookService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCodebookService(this WebApplicationBuilder builder)
    {
        // add general Dapper repository
        builder.Services
            .AddDapper(builder.Configuration.GetConnectionString("default")!)
            .AddDapper<IXxdDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxd")!)
            .AddDapper<IXxdHfDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxdhf")!)
            .AddDapper<IKonsdbDapperConnectionProvider>(builder.Configuration.GetConnectionString("konsDb")!);
        
        // seznam SQL dotazu
        builder.Services.AddSingleton(provider =>
        {
            var database = provider.GetRequiredService<CIS.Core.Data.IConnectionProvider>();
            var data = database
                .ExecuteDapperRawSqlToList<(string SqlQueryId, string SqlQueryText, SqlQueryCollection.DatabaseProviders DatabaseProvider)>("SELECT SqlQueryId, SqlQueryText, DatabaseProvider FROM dbo.SqlQuery")
                .ToDictionary(k => k.SqlQueryId, v => new SqlQueryCollection.QueryItem { Provider = v.DatabaseProvider, Query = v.SqlQueryText });

            return new SqlQueryCollection(data);
        });

        return builder;
    }
}
