using CIS.Infrastructure.StartupExtensions;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

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

        builder.Services.AddSingleton(provider =>
        {
            Database.SqlQueryCollection colletion = new Database.SqlQueryCollection(null);
            var database = provider.GetRequiredService<CIS.Core.Data.IConnectionProvider>();
            return colletion;
        });

        return builder;
    }
}
