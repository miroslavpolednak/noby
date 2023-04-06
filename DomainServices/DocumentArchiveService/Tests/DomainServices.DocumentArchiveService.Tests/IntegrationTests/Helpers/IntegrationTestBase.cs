using DomainServices.UserService.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using _CIS = CIS.Infrastructure.gRPC.CisTypes;
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
using Microsoft.EntityFrameworkCore;
using DomainServices.DocumentArchiveService.Api.Database;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;

public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    //Mocks
    internal IDocumentSequenceRepository DocumentSequenceRepository { get; }
    internal IUserServiceClient UserServiceClient { get; }

    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        DocumentSequenceRepository = Substitute.For<IDocumentSequenceRepository>();
        UserServiceClient = Substitute.For<IUserServiceClient>();

        ConfigureWebHost();

        CreateGlobalMocks();
    }

    public WebApplicationFactoryFixture<Program> Fixture { get; }

    protected DocumentArchiveServiceClient CreateGrpcClient()
    {
        return Fixture.CreateGrpcClient<DocumentArchiveServiceClient>();
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

    protected static User CreateUser(
        int id = 3048,
        string czechIdentificationNumber = "12345678",
        string fullName = "Filip Tůma",
        string CPM = "99614w",
        string identity = """{ "identity": "990614w", "identityScheme": "Mpad" }""",
        _CIS.UserIdentity.Types.UserIdentitySchemes dentityScheme = _CIS.UserIdentity.Types.UserIdentitySchemes.Mpad
        )
    {
        var user = new User
        {
            Id = id,
            CzechIdentificationNumber = czechIdentificationNumber,
            FullName = fullName,
            CPM = CPM,
        };

        user.UserIdentifiers.Add(new _CIS.UserIdentity
        {
            Identity = identity,
            IdentityScheme = dentityScheme

        });
        return user;
    }

    private void ConfigureWebHost()
    {
        Fixture
       .ConfigureServices(services =>
        {
            // Use exist mock of sdf 
            services.RemoveAll<ISdfClient>().AddSingleton<ISdfClient, MockSdfClient>();

            // Use exist mock of Tcp
            services.RemoveAll<IDocumentServiceRepository>().AddSingleton<IDocumentServiceRepository, MockDocumentServiceRepository>();
            services.RemoveAll<ITcpClient>().AddSingleton<ITcpClient, TcpClientMock>();

            // mock dapper call repository
            services.RemoveAll<IDocumentSequenceRepository>().AddSingleton(DocumentSequenceRepository);

            // Mock of grpc call to other grpc services
            services.RemoveAll<IUserServiceClient>().AddSingleton(UserServiceClient);

            var dbName = Guid.NewGuid().ToString();// unique db name for every test class
            services.RemoveAll<DbContextOptions<DocumentArchiveDbContext>>()
                  .AddDbContext<DocumentArchiveDbContext>(options =>
                  {
                      options.UseInMemoryDatabase(dbName);
                      options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                  });
        });
    }
    private void CreateGlobalMocks()
    {
        var user = CreateUser();
        UserServiceClient.GetUser(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(user);
    }
}
