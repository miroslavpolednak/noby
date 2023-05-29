using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Data;

namespace DomainServices.CodebookService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCodebookService(this WebApplicationBuilder builder)
    {
        // add general Dapper repository
        builder.Services
            .AddDapper(builder.Configuration.GetConnectionString("default")!)
            .AddDapper<Database.IXxdDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxd")!)
            .AddDapper<Database.IXxdHfDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxdhf")!)
            .AddDapper<Database.IKonsdbDapperConnectionProvider>(builder.Configuration.GetConnectionString("konsDb")!);
        
        // seznam SQL dotazu
        builder.Services.AddSingleton(provider =>
        {
            var database = provider.GetRequiredService<CIS.Core.Data.IConnectionProvider>();
            var data = database
                .ExecuteDapperRawSqlToList<(string SqlQueryId, string SqlQueryText)>("SELECT SqlQueryId, SqlQueryText FROM dbo.SqlQuery")
                .ToDictionary(k => k.SqlQueryId, v => v.SqlQueryText);
            return new Database.SqlQueryCollection(data);
        });

        return builder;
    }
}
