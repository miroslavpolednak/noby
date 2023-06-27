using AutoFixture;
using AutoFixture.AutoMoq;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Handlers.Result;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Tests.Mocks;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.Testing.Common;
using Moq;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.GetResult;

public class GetResultTests
{
    private readonly Fixture _fixture;
    
    public GetResultTests()
    {
        _fixture = FixtureFactory.Create();
        _fixture.Customize(new AutoMoqCustomization());

        _fixture.MockAppConfig();
        _fixture.MockCodebookService();
        _fixture.MockRepository();
    }

    [Fact]
    public async Task GetSmsResult()
    {
        _fixture.MockUserAdapterService("UsernameA");
        
        var guid = RepositoryExtensions.SmsResultId1;
        var token = CancellationToken.None;
        var request = new GetResultRequest { NotificationId = guid };
        
        var handler = _fixture.Create<GetResultHandler>();
        var response = await handler.Handle(request, token);
        
        Assert.Equal(guid, response.Result.NotificationId);
        Assert.Equal(NotificationChannel.Sms, response.Result.Channel);
        Assert.Equal(NotificationState.InProgress, response.Result.State);
        Assert.Equal("UsernameA", response.Result.CreatedBy);
        Assert.Equal("CustomIdA", response.Result.CustomId);
        Assert.Null(response.Result.DocumentId);
        Assert.Null(response.Result.Identifier);
        Assert.Empty(response.Result.Errors);
        Assert.Equal(new DateTime(2023, 1, 1, 1, 0, 0), response.Result.RequestTimestamp);
        Assert.Null(response.Result.ResultTimestamp);
        
        Assert.NotNull(response.Result.RequestData);
        Assert.Null(response.Result.RequestData.EmailData);
        Assert.NotNull(response.Result.RequestData.SmsData);
        Assert.Equal("+420", response.Result.RequestData.SmsData.Phone.CountryCode);
        Assert.Equal("777123456", response.Result.RequestData.SmsData.Phone.NationalNumber);
        
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        mockRepository.Verify(r => r.GetResult(guid, token), Times.Once);
    }

    [Fact]
    public async Task GetEmailResult()
    {
        _fixture.MockUserAdapterService("UsernameA");
        
        var guid = RepositoryExtensions.EmailResultId1;
        var token = CancellationToken.None;
        var request = new GetResultRequest { NotificationId = guid };
        
        var handler = _fixture.Create<GetResultHandler>();
        var response = await handler.Handle(request, token);
        
        Assert.Equal(guid, response.Result.NotificationId);
        Assert.Equal(NotificationChannel.Email, response.Result.Channel);
        Assert.Equal(NotificationState.InProgress, response.Result.State);
        Assert.Equal("UsernameA", response.Result.CreatedBy);
        Assert.Equal("CustomIdA", response.Result.CustomId);
        Assert.Null(response.Result.DocumentId);
        Assert.Null(response.Result.Identifier);
        Assert.Empty(response.Result.Errors);
        Assert.Equal(new DateTime(2023, 1, 1, 1, 1, 0), response.Result.RequestTimestamp);
        Assert.Null(response.Result.ResultTimestamp);
        
        Assert.NotNull(response.Result.RequestData);
        Assert.NotNull(response.Result.RequestData.EmailData);
        Assert.Null(response.Result.RequestData.SmsData);

        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        mockRepository.Verify(r => r.GetResult(guid, token), Times.Once);
    }

    [Fact]
    public async Task GetNotExistingResult()
    {
        _fixture.MockUserAdapterService("UsernameA");
        
        var guid = Guid.NewGuid();
        var token = CancellationToken.None;
        var request = new GetResultRequest { NotificationId = guid };
        
        var handler = _fixture.Create<GetResultHandler>();
        
        var exception = await Assert.ThrowsAsync<CisNotFoundException>(async () =>
        {
            await handler.Handle(request, token);
        });

        Assert.Equal("Result not found.", exception.Message);
        
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        mockRepository.Verify(r => r.GetResult(guid, token), Times.Once);
    }
}