using AutoFixture;
using AutoFixture.AutoMoq;
using CIS.InternalServices.NotificationService.Api.Handlers.Result;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Tests.Mocks;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.Testing.Common;
using Moq;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.SearchResults;

public class SearchResultsTests
{
    private readonly Fixture _fixture;
    public SearchResultsTests()
    {
        _fixture = FixtureFactory.Create();
        _fixture.Customize(new AutoMoqCustomization());

        _fixture.MockAppConfig();
        _fixture.MockCodebookService();
        _fixture.MockRepository();
        _fixture.MockUserAdapterService("UsernameA");
    }
    
    [Fact]
    public async Task SearchByCustomId()
    {
        var token = CancellationToken.None;
        var request = new SearchResultsRequest { CustomId = "CustomIdA" };
        
        var handler = _fixture.Create<SearchResultsHandler>();
        var response = await handler.Handle(request, token);
        
        Assert.Equal(2, response.Results.Count);
        Assert.Single(response.Results.Where(r => r.NotificationId == RepositoryExtensions.SmsResultId1));
        Assert.Single(response.Results.Where(r => r.NotificationId == RepositoryExtensions.EmailResultId1));
        
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        mockRepository.Verify(r =>
            r.SearchResultsBy(null, null, "CustomIdA", null),
            Times.Once);
    }
    
    [Fact]
    public async Task SearchByDocumentId()
    {
        var token = CancellationToken.None;
        var request = new SearchResultsRequest { DocumentId = "DocumentIdA" };
        
        var handler = _fixture.Create<SearchResultsHandler>();
        var response = await handler.Handle(request, token);
        
        Assert.Equal(2, response.Results.Count);
        Assert.Single(response.Results.Where(r => r.NotificationId == RepositoryExtensions.SmsResultId2));
        Assert.Single(response.Results.Where(r => r.NotificationId == RepositoryExtensions.EmailResultId2));
        
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        mockRepository.Verify(r =>
            r.SearchResultsBy(null, null, null, "DocumentIdA"),
            Times.Once);
    }
    
    [Fact]
    public async Task SearchByIdentifier()
    {
        var token = CancellationToken.None;
        var request = new SearchResultsRequest { Identity = "IdentityA", IdentityScheme = "IdentitySchemeA" };
        
        var handler = _fixture.Create<SearchResultsHandler>();
        var response = await handler.Handle(request, token);
        
        Assert.Equal(2, response.Results.Count);
        Assert.Single(response.Results.Where(r => r.NotificationId == RepositoryExtensions.SmsResultId3));
        Assert.Single(response.Results.Where(r => r.NotificationId == RepositoryExtensions.EmailResultId3));
        
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        mockRepository.Verify(r =>
            r.SearchResultsBy("IdentityA", "IdentitySchemeA", null, null),
            Times.Once);
    }
}