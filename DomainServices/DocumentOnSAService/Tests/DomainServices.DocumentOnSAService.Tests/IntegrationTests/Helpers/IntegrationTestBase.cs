using SharedTypes.Enums;
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
using ExternalServices.ESignatures.V1;
using _Domain = DomainServices.DocumentOnSAService.Contracts;
using SharedTypes.GrpcTypes;
using DomainServices.CaseService.Clients;
using DomainServices.CustomerService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using DomainServices.DocumentOnSAService.Api.Common;
using DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;

namespace DomainServices.DocumentOnSAService.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    //Mocks
    internal IDataAggregatorServiceClient DataAggregatorServiceClient { get; }
    internal ISalesArrangementServiceClient ArrangementServiceClient { get; }
    internal IHouseholdServiceClient HouseholdServiceClient { get; }
    internal IDocumentArchiveServiceClient DocumentArchiveServiceClient { get; }
    internal IEasClient EasClient { get; }
    internal IESignaturesClient ESignaturesClient { get; }
    internal ICustomerOnSAServiceClient CustomerOnSAServiceClient { get; }
    internal IProductServiceClient ProductServiceClient { get; }
    internal ICaseServiceClient CaseServiceClient { get; }
    internal ICustomerServiceClient CustomerServiceClient { get; }
    internal IDocumentGeneratorServiceClient DocumentGeneratorServiceClient { get; }
    internal ISalesArrangementStateManager SalesArrangementStateManager { get; }
    internal ISbQueuesRepository SbQueuesRepository { get; }

    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        DataAggregatorServiceClient = Substitute.For<IDataAggregatorServiceClient>();
        ArrangementServiceClient = Substitute.For<ISalesArrangementServiceClient>();
        HouseholdServiceClient = Substitute.For<IHouseholdServiceClient>();
        DocumentArchiveServiceClient = Substitute.For<IDocumentArchiveServiceClient>();
        EasClient = Substitute.For<IEasClient>();
        ESignaturesClient = Substitute.For<IESignaturesClient>();
        CustomerOnSAServiceClient = Substitute.For<ICustomerOnSAServiceClient>();
        ProductServiceClient = Substitute.For<IProductServiceClient>();
        CaseServiceClient = Substitute.For<ICaseServiceClient>();
        CustomerServiceClient = Substitute.For<ICustomerServiceClient>();
        DocumentGeneratorServiceClient = Substitute.For<IDocumentGeneratorServiceClient>();
        SalesArrangementStateManager = Substitute.For<ISalesArrangementStateManager>();
        SbQueuesRepository = Substitute.For<ISbQueuesRepository>(); 
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
            services.RemoveAll<IDataAggregatorServiceClient>().AddScoped(_ => DataAggregatorServiceClient);
            services.RemoveAll<ISalesArrangementServiceClient>().AddScoped(_ => ArrangementServiceClient);
            services.RemoveAll<IHouseholdServiceClient>().AddScoped(_ => HouseholdServiceClient);
            services.RemoveAll<IDocumentArchiveServiceClient>().AddScoped(_ => DocumentArchiveServiceClient);
            services.RemoveAll<IEasClient>().AddScoped(_ => EasClient);
            services.RemoveAll<IESignaturesClient>().AddScoped(_ => ESignaturesClient);
            services.RemoveAll<ICustomerOnSAServiceClient>().AddScoped(_ => CustomerOnSAServiceClient);
            services.RemoveAll<IProductServiceClient>().AddScoped(_ => ProductServiceClient);
            services.RemoveAll<ICaseServiceClient>().AddScoped(_ => CaseServiceClient);
            services.RemoveAll<ICustomerServiceClient>().AddScoped(_ => CustomerServiceClient);
            services.RemoveAll<IDocumentGeneratorServiceClient>().AddScoped(_ => DocumentGeneratorServiceClient);
            services.RemoveAll<ISalesArrangementStateManager>().AddScoped(_ => SalesArrangementStateManager);
            services.RemoveAll<ISbQueuesRepository>().AddScoped(_ => SbQueuesRepository);
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


    protected _Domain.SigningIdentity CreateSigningIdentity(
        int customerOnSaId = 4522
        )
    {
        var identities = new List<Identity> { new Identity { IdentityId = 1, IdentityScheme = Identity.Types.IdentitySchemes.Kb } };

        return new _Domain.SigningIdentity
        {
            BirthNumber = "8808221117",
            FirstName = "Pavel",
            LastName = "Novák",
            CustomerIdentifiers = { identities },
            EmailAddress = "test@gmail.com",
            MobilePhone = new() { PhoneIDC = "+420", PhoneNumber = "725966844" },
            CustomerOnSAId = customerOnSaId,
            SignatureDataCode = "SIG_X_1"
        };
    }

    protected DocumentOnSa CreateDocOnSaEntity(
        int documentTemplateVersionId = 4,
        string formId = "N00000000000199",
        string eArchivId = "KBHXXD00000000000000000000351",
        int salesArrangementId = 1,
        int? householdId = 1,
        int signatureTypeId = (int)SignatureTypes.Paper,
        int documentTypeId = 1,
        int customerOnSAId1 = 1,
        int customerOnSAId2 = 2
        )
    {

        var docOnSa = new DocumentOnSa
        {
            DocumentTemplateVersionId = documentTemplateVersionId,
            DocumentTypeId = documentTypeId,
            FormId = formId,
            EArchivId = eArchivId,
            SalesArrangementId = salesArrangementId,
            HouseholdId = householdId,
            IsValid = true,
            IsSigned = false,
            CreatedUserName = "MPSSRootTest",
            CreatedUserId = 1,
            CreatedTime = DateTime.Now,
            DocumentTemplateVariantId = 1,
            ExternalId = "SomeExternalId",
            Source = Api.Database.Enums.Source.Noby,
            SignatureTypeId = signatureTypeId,
            CustomerOnSAId1 = customerOnSAId1,
            CustomerOnSAId2 = customerOnSAId2,
            Data = """{"FieldName":"RodneCisloText","StringFormat":"Rodn\u00E9 \u010D\u00EDslo:","Text":"8151016302","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"RodneCislo","StringFormat":null,"Text":"8151016302","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"DatumNarozeniText","StringFormat":"","Text":"","Date":{"Year":1981,"Month":1,"Day":1},"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":4},{"FieldName":"DatumNarozeni","StringFormat":"","Text":"","Date":{"Year":1981,"Month":1,"Day":1},"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":4},{"FieldName":"UcetKeSplaceniText","StringFormat":"Jsem si v\u011Bdom/a toho, \u017Ee pokud nejsem majitelem \u00FA\u010Dtu uveden\u00E9ho pro spl\u00E1cen\u00ED \u00FAv\u011Bru, je p\u0159ed prvn\u00EDm \u010Cerp\u00E1n\u00EDm nutn\u00E9 dolo\u017Eit souhlas majitele tohoto \u00FA\u010Dtu.","Text":"","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":0},{"FieldName":"Podpis","StringFormat":"Podpis na z\u00E1klad\u011B pln\u00E9 moci","Text":"","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":0},{"FieldName":"PodpisKlient","StringFormat":"","Text":"","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":0},{"FieldName":"PodpisZmocnenec","StringFormat":"Jm\u00E9no zmocn\u011Bnce:","Text":"","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":0},{"FieldName":"CisloUverovahoUctu","StringFormat":null,"Text":"35-2271460227/0100 a to bezokladn\u011B.","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"JmenoPrijmeni","StringFormat":null,"Text":"JANA NOV\u00C1KOV\u00C1, MVDR.","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"PodpisJmenoZmocnence","StringFormat":null,"Text":"Marek Nov\u00E1k","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"TrvalyPobyt","StringFormat":null,"Text":"Bryksova 666/4, 19800 Praha 9 - \u010Cern\u00FD Most, \u010Cesk\u00E1 republika","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"UcetKeSplaceni","StringFormat":null,"Text":"\u010C\u00EDslo \u00FA\u010Dtu pro spl\u00E1cen\u00ED \u00FAv\u011Bru: -3510720263/0100","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"CastkaVKc1","StringFormat":"{0:#,#.##}","Text":"","Date":null,"Number":0,"DecimalNumber":{"Units":10000,"Nanos":0},"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":6},{"FieldName":"CisloUctu1","StringFormat":null,"Text":"32101084","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"KodBanky1","StringFormat":null,"Text":"0100","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"VariabilniSymbol1","StringFormat":null,"Text":"789","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"KonstantniSymbol1","StringFormat":null,"Text":"123","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3},{"FieldName":"SpecifickySymbol1","StringFormat":null,"Text":"46","Date":null,"Number":0,"DecimalNumber":null,"LogicalValue":false,"Table":null,"TextAlign":null,"ValueCase":3}"""
        };
        docOnSa.EArchivIdsLinkeds.Add(new EArchivIdsLinked { EArchivId = eArchivId });
        return docOnSa;
    }
}
