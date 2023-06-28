using CIS.Testing;
using CIS.Testing.Database;
using DomainServices.UserService.Clients.Services;
using DomainServices.UserService.Clients;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.DocumentOnSAService.Api.Database;
using static DomainServices.DocumentOnSAService.Contracts.v1.DocumentOnSAService;
using CIS.InternalServices.DataAggregatorService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NSubstitute;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;
using CIS.Infrastructure.BackgroundServices;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using ExternalServices.Eas.V1;
using DomainServices.ProductService.Clients;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    //Mocks
    internal IDataAggregatorServiceClient DataAggregatorServiceClient { get; }
    internal ISalesArrangementServiceClient ArrangementServiceClient { get; }
    internal IHouseholdServiceClient HouseholdServiceClient { get; }
    internal IDocumentArchiveServiceClient DocumentArchiveServiceClient { get; }
    internal IEasClient EasClient { get; }
    internal ICustomerOnSAServiceClient CustomerOnSAServiceClient { get; }
    internal IProductServiceClient ProductServiceClient { get; }

    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        DataAggregatorServiceClient = Substitute.For<IDataAggregatorServiceClient>();
        ArrangementServiceClient = Substitute.For<ISalesArrangementServiceClient>();
        HouseholdServiceClient = Substitute.For<IHouseholdServiceClient>();
        DocumentArchiveServiceClient = Substitute.For<IDocumentArchiveServiceClient>();
        EasClient = Substitute.For<IEasClient>();
        CustomerOnSAServiceClient = Substitute.For<ICustomerOnSAServiceClient>();
        ProductServiceClient = Substitute.For<IProductServiceClient>();

        ConfigureWebHost();

        PrepareDatabase();
    }
    public WebApplicationFactoryFixture<Program> Fixture { get; }

    private void ConfigureWebHost()
    {
        Fixture.ConfigureCisTestOptions(options =>
        {
            // Need SqlLite db, for sequence testing
            options.DbMockAdapter = new SqliteInMemoryMockAdapter();
        })
        .ConfigureServices(services =>
        {
            // Disable job execution
            services.RemoveAll<ICisBackgroundServiceConfiguration<CheckDocumentsArchivedJob>>().AddSingleton<ICisBackgroundServiceConfiguration<CheckDocumentsArchivedJob>>(new CisBackgroundServiceConfiguration<CheckDocumentsArchivedJob>
            {
                CronSchedule = "* * * * *",
                Disabled = true
            });

            // This mock is necessary for mock of service discovery
            services.RemoveAll<IUserServiceClient>().AddSingleton<IUserServiceClient, MockUserService>();

            services.RemoveAll<ICodebookServiceClient>().AddSingleton<ICodebookServiceClient, CodebookService.Clients.Services.CodebookServiceMock>();

            // NSubstitute mocks
            services.RemoveAll<IDataAggregatorServiceClient>().AddTransient(r => DataAggregatorServiceClient);
            services.RemoveAll<ISalesArrangementServiceClient>().AddTransient(r => ArrangementServiceClient);
            services.RemoveAll<IHouseholdServiceClient>().AddTransient(r => HouseholdServiceClient);
            services.RemoveAll<IDocumentArchiveServiceClient>().AddTransient(r => DocumentArchiveServiceClient);
            services.RemoveAll<IEasClient>().AddTransient(r => EasClient);
            services.RemoveAll<ICustomerOnSAServiceClient>().AddTransient(r => CustomerOnSAServiceClient);
            services.RemoveAll<IProductServiceClient>().AddTransient(r=> ProductServiceClient);
        });
    }

    protected DocumentOnSAServiceClient CreateGrpcClient()
    {
        return Fixture.CreateGrpcClient<DocumentOnSAServiceClient>(true);
    }

    protected void PrepareDatabase()
    {
        var scope = Fixture.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DocumentOnSAServiceDbContext>();
        context.Database.EnsureCreated();
    }

    protected DocumentOnSa CreateDocOnSaEntity(
        int documentTemplateVersionId = 4,
        string formId = "N00000000000199",
        string eArchivId = "KBHXXD00000000000000000000351",
        int salesArrangementId = 1,
        int householdId = 1,
        int signatureTypeId = 1
        )
    {
        return new DocumentOnSa
        {
            DocumentTemplateVersionId = documentTemplateVersionId,
            DocumentTypeId = 1,
            FormId = formId,
            EArchivId = eArchivId,
            SalesArrangementId = salesArrangementId,
            HouseholdId = householdId,
            IsValid = true,
            IsSigned = false,
            SignatureMethodCode = "PHYSICAL",
            CreatedUserName = "MPSSRootTest",
            CreatedUserId = 1,
            CreatedTime = DateTime.Now,
            DocumentTemplateVariantId = 1,
            ExternalId="SomeExternalId",
            Source = Api.Database.Enums.Source.Noby,
            SignatureTypeId = signatureTypeId,
            Data = """{"FieldName":"RodneCisloText","StringFormat":"Rodn\u00E9 \u010D\u00EDslo:","Text":"8151016302","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"RodneCislo","StringFormat":null,"Text":"8151016302","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"DatumNarozeniText","StringFormat":"","Text":"","Date":{"Year":1981,"Month":1,"Day":1},"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":4},{"FieldName":"DatumNarozeni","StringFormat":"","Text":"","Date":{"Year":1981,"Month":1,"Day":1},"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":4},{"FieldName":"UcetKeSplaceniText","StringFormat":"Jsem si v\u011Bdom/a toho, \u017Ee pokud nejsem majitelem \u00FA\u010Dtu uveden\u00E9ho pro spl\u00E1cen\u00ED \u00FAv\u011Bru, je p\u0159ed prvn\u00EDm \u010Cerp\u00E1n\u00EDm nutn\u00E9 dolo\u017Eit souhlas majitele tohoto \u00FA\u010Dtu.","Text":"","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":0},{"FieldName":"Podpis","StringFormat":"Podpis na z\u00E1klad\u011B pln\u00E9 moci","Text":"","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":0},{"FieldName":"PodpisKlient","StringFormat":"","Text":"","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":0},{"FieldName":"PodpisZmocnenec","StringFormat":"Jm\u00E9no zmocn\u011Bnce:","Text":"","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":0},{"FieldName":"CisloUverovahoUctu","StringFormat":null,"Text":"35-2271460227/0100 a to bezokladn\u011B.","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"JmenoPrijmeni","StringFormat":null,"Text":"JANA NOV\u00C1KOV\u00C1, MVDR.","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"PodpisJmenoZmocnence","StringFormat":null,"Text":"Marek Nov\u00E1k","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"TrvalyPobyt","StringFormat":null,"Text":"Bryksova 666/4, 19800 Praha 9 - \u010Cern\u00FD Most, \u010Cesk\u00E1 republika","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"UcetKeSplaceni","StringFormat":null,"Text":"\u010C\u00EDslo \u00FA\u010Dtu pro spl\u00E1cen\u00ED \u00FAv\u011Bru: -3510720263/0100","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"CastkaVKc1","StringFormat":"{0:#,#.##}","Text":"","Date":null,"Number":0,"DecimalNumber":{"Units":10000,"Nanos":0},"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":6},{"FieldName":"CisloUctu1","StringFormat":null,"Text":"32101084","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"KodBanky1","StringFormat":null,"Text":"0100","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"VariabilniSymbol1","StringFormat":null,"Text":"789","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"KonstantniSymbol1","StringFormat":null,"Text":"123","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"SpecifickySymbol1","StringFormat":null,"Text":"46","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3}"""
        };

    }
}
