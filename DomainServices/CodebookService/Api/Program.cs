using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Data;
using DomainServices.CodebookService.Api.Database;
using DomainServices.CodebookService.ExternalServices.AcvEnumService.V1;
using DomainServices.CodebookService.ExternalServices.RDM.V1;
using DomainServices.CodebookService.ExternalServices;

SharedComponents.GrpcServiceBuilder
    .CreateGrpcService(args, typeof(Program))
    .AddApplicationConfiguration<DomainServices.CodebookService.Api.Configuration.AppConfiguration>()
    .EnableJsonTranscoding(options =>
    {
        options.OpenApiTitle = "Codebook Service API";
        options.AddOpenApiXmlCommentFromBaseDirectory("DomainServices.CodebookService.Contracts.xml");
    })
    .SkipServiceUserContext()
    .AddErrorCodeMapper(DomainServices.CodebookService.Api.ErrorCodeMapper.Init())
    .Build(builder =>
    {
        const string _sqlQuerySelect = "SELECT SqlQueryId, SqlQueryText, DatabaseProvider FROM dbo.SqlQuery";

        // add general Dapper repository
        builder.Services
            .AddDapper(builder.Configuration.GetConnectionString(CisGlobalConstants.DefaultConnectionStringKey)!)
            .AddDapper<IXxdHfDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxdhf")!)
            .AddDapper<IKonsdbDapperConnectionProvider>(builder.Configuration.GetConnectionString("konsDb")!);

        // seznam SQL dotazu
        builder.Services.AddSingleton(provider =>
        {
            var database = provider.GetRequiredService<CIS.Core.Data.IConnectionProvider>();
            var data = database
                .ExecuteDapperRawSqlToList<(string SqlQueryId, string SqlQueryText, SqlQueryCollection.DatabaseProviders DatabaseProvider)>(_sqlQuerySelect)
                .ToDictionary(k => k.SqlQueryId, v => new SqlQueryCollection.QueryItem { Provider = v.DatabaseProvider, Query = v.SqlQueryText });

            return new SqlQueryCollection(data);
        });

        // background svc
        builder.AddCisBackgroundService<DomainServices.CodebookService.Api.BackgroundServices.DownloadRdmCodebooksJob.DownloadRdmCodebooksJob>();

        builder.AddExternalService<IAcvEnumServiceClient>();
        builder.AddExternalService<IRDMClient>();
    })
    .MapGrpcServices(app =>
    {
        app.MapGrpcService<DomainServices.CodebookService.Api.Endpoints.CodebookService>();
    })
    .Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}