﻿using CIS.Core.Exceptions.ExternalServices;
using CIS.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using static DomainServices.CaseService.Contracts.v1.CaseService;

namespace DomainServices.CaseService.Tests.IntegrationTests;

public abstract class IntegrationTestBase 
    : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    protected Mock<ExternalServices.SbWebApi.V1.ISbWebApiClient> MockSbWebApi { get; private set; }
    protected Mock<global::ExternalServices.Eas.V1.IEasClient> MockEas { get; private set; }

    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        configureWebHost();

        MockSbWebApi = new Mock<ExternalServices.SbWebApi.V1.ISbWebApiClient>();
        MockSbWebApi
            .Setup(t => t.CancelTask(It.Is<int>(i => i == 1), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        MockSbWebApi
            .Setup(t => t.CancelTask(It.Is<int>(i => i == 2), It.IsAny<CancellationToken>()))
            .Throws(new CisExternalServiceValidationException(2, "exception"));
        MockSbWebApi
            .Setup(t => t.FindTasksByTaskId(It.IsAny<ExternalServices.SbWebApi.Dto.FindTasks.FindByTaskIdRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IList<IReadOnlyDictionary<string, string>>>(new List<IReadOnlyDictionary<string, string>>()));

        MockEas = new Mock<global::ExternalServices.Eas.V1.IEasClient>();

        PrepareDatabase();
    }

    protected WebApplicationFactoryFixture<Program> Fixture { get; }

    protected CaseServiceClient CreateGrpcClient()
    {
        return Fixture.CreateGrpcClient<CaseServiceClient>();
    }

    private void configureWebHost()
    {
        Fixture
            .ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Transient<UserService.Clients.v1.IUserServiceClient, UserService.Clients.v1.MockUserServiceClient>());
                services.Replace(ServiceDescriptor.Transient<CodebookService.Clients.ICodebookServiceClient, CodebookService.Clients.Services.CodebookServiceMock>());

                var sa = new Mock<SalesArrangementService.Clients.ISalesArrangementServiceClient>();
                var doc = new Mock<DocumentOnSAService.Clients.IDocumentOnSAServiceClient>();
                
                services.Replace(new ServiceDescriptor(typeof(SalesArrangementService.Clients.ISalesArrangementServiceClient), t => sa.Object, ServiceLifetime.Transient));
                services.Replace(new ServiceDescriptor(typeof(DocumentOnSAService.Clients.IDocumentOnSAServiceClient), t => doc.Object, ServiceLifetime.Transient));
                services.Replace(new ServiceDescriptor(typeof(global::ExternalServices.Eas.V1.IEasClient), t => MockEas.Object, ServiceLifetime.Scoped));
                services.Replace(new ServiceDescriptor(typeof(ExternalServices.SbWebApi.V1.ISbWebApiClient), t => MockSbWebApi.Object, ServiceLifetime.Scoped));
            });
    }

    private void PrepareDatabase()
    {
        using var scope = Fixture.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Api.Database.CaseServiceDbContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}
