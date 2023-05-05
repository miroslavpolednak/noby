using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using static DomainServices.DocumentArchiveService.Contracts.v1.DocumentArchiveService;
using DomainServices.DocumentArchiveService.Api.Database.Entities;
using DomainServices.DocumentArchiveService.Api.Database.Repositories;
using DomainServices.UserService.Clients;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Clients;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Clients;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Repositories;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.Testing;
using DomainServices.UserService.Clients.Services;
using DomainServices.DocumentArchiveService.Api.Database;
using CIS.Testing.Database;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    //Mocks
    internal IDocumentSequenceRepository DocumentSequenceRepository { get; }

    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        DocumentSequenceRepository = Substitute.For<IDocumentSequenceRepository>();

        ConfigureWebHost();

        PrepareDatabase();
    }

    public WebApplicationFactoryFixture<Program> Fixture { get; }

    protected DocumentArchiveServiceClient CreateGrpcClient()
    {
        return Fixture.CreateGrpcClient<DocumentArchiveServiceClient>(true);
    }

    private void ConfigureWebHost()
    {
        Fixture
        // This configuration is optional, everything is set correctly by default.
       .ConfigureCisTestOptions(options =>
       {
           // default is EfInMemoryMockAdapter
           options.DbMockAdapter = new SqliteInMemoryMockAdapter();
           // If you set this property to false, you have to manually register in memory database, if you don't do it, regular database gonna be used and this is terrible wrong.  
           // Example of how to do it manually is shown below
           options.UseDbContextAutoMock = true; // default
           options.UseNullLogger = true; // default
           options.UseNobyAuthenticationHeader = true; // default
           // Example of custom header, it is necessary create new instance  
           options.Header = new() { { "test", "Test" } }; // default is null
       })
       .ConfigureServices(services =>
       {
           // This mock is necessary for mock of service discovery
           services.RemoveAll<IUserServiceClient>().AddSingleton<IUserServiceClient, MockUserService>();

           // Use exist mock of sdf 
           services.RemoveAll<ISdfClient>().AddSingleton<ISdfClient, MockSdfClient>();

           // Use exist mock of Tcp
           services.RemoveAll<IDocumentServiceRepository>().AddSingleton<IDocumentServiceRepository, MockDocumentServiceRepository>();
           services.RemoveAll<ITcpClient>().AddSingleton<ITcpClient, TcpClientMock>();

           // mock dapper call repository
           services.RemoveAll<IDocumentSequenceRepository>().AddSingleton(DocumentSequenceRepository);

           // Example of manual register of db context with inmemory database
           //var dbName = Guid.NewGuid().ToString();// unique db name for every test class
           //services.RemoveAll<DbContextOptions<DocumentArchiveDbContext>>()
           //      .AddDbContext<DocumentArchiveDbContext>(options =>
           //      {
           //          options.UseInMemoryDatabase(dbName);
           //          options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
           //      });
       });
    }

    protected DocumentInterface CreateDocumentInterfaceEntity(
            string documentId = "KBHXXD00000000000000000000001",
            byte[]? documentData = null,
            string fileName = "test.txt",
            string fileNameSuffix = "txt",
            string description = "test desc",
            long caseId = 1,
            DateTime? createdOn = null,
            string authorUserLogin = "testUser",
            string contractNumber = "HF00000000001",
            string formId = "N00000000000001",
            int eaCodeMainId = 613226,
            string documentDirection = "E",
            string folderDocument = "N"
            )
    {
        return new DocumentInterface()
        {
            DocumentId = documentId,
            DocumentData = documentData ?? Convert.FromBase64String("VGhpcyBpcyBhIHRlc3Q="),
            FileName = fileName,
            FileNameSuffix = fileNameSuffix,
            Description = description,
            CaseId = caseId,
            CreatedOn = createdOn ?? new DateTime(2022, 1, 1),
            AuthorUserLogin = authorUserLogin,
            ContractNumber = contractNumber,
            FormId = formId,
            EaCodeMainId = eaCodeMainId,
            DocumentDirection = documentDirection,
            FolderDocument = folderDocument
        };
    }

    private void PrepareDatabase()
    {
        var scope = Fixture.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DocumentArchiveDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
