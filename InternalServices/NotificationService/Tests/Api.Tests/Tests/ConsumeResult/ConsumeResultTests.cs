using AutoFixture;
using AutoFixture.AutoMoq;
using CIS.InternalServices.NotificationService.Api.Handlers.Result;
using CIS.InternalServices.NotificationService.Api.Handlers.Result.Requests;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Tests.Mocks;
using CIS.Testing.Common;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.notificationreport;
using cz.kb.osbs.mcs.notificationreport.eventapi.v3.report;
using Moq;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.ConsumeResult;

public class ConsumeResultTests
{
    private readonly Fixture _fixture;
    
    public ConsumeResultTests()
    {
        _fixture = FixtureFactory.Create();
        _fixture.Customize(new AutoMoqCustomization());
        
        _fixture.MockServiceProvider();
        _fixture.MockAppConfig();
        _fixture.MockCodebookService();
        _fixture.MockRepository();
        _fixture.MockUserAdapterService("UsernameA");
    }

    [Fact]
    public async Task ConsumeExistingResult()
    {
        var token = CancellationToken.None;
        var request = new ConsumeResultRequest
        {
            NotificationReport = new NotificationReport 
            {
                id = RepositoryExtensions.SmsResultId2.ToString(),
                channel = new (){ id = "SMS" },
                state = "SENT",
                notificationErrors = new List<NotificationError>()
            }
        };
        
        var handler = _fixture.Create<ConsumeResultHandler>();
        await handler.Handle(request, token);
        
        // todo:
    }

    [Fact]
    public async Task ConsumeNotExistingResult()
    {
        var id = Guid.NewGuid();
        var token = CancellationToken.None;
        var request = new ConsumeResultRequest
        {
            NotificationReport = new NotificationReport 
            {
                id = id.ToString(),
                channel = new (){ id = "SMS" },
                state = "SENT",
                notificationErrors = new List<NotificationError>()
            }
        };
        
        var handler = _fixture.Create<ConsumeResultHandler>();
        await handler.Handle(request, token);
        
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        mockRepository.Verify(m => m.GetResult(id, token), Times.Once);
        mockRepository.Verify(m => m.SaveChanges(token), Times.Never);
        
        // todo:
    }

    [Fact]
    public async Task ConsumeSmsResultWithAudit()
    {
        var token = CancellationToken.None;
        var request = new ConsumeResultRequest
        {
            NotificationReport = new NotificationReport 
            {
                id = RepositoryExtensions.SmsResultId2.ToString(),
                channel = new (){ id = "SMS" },
                state = "SENT",
                notificationErrors = new List<NotificationError>()
            }
        };
        
        var handler = _fixture.Create<ConsumeResultHandler>();
        await handler.Handle(request, token);
        
        // todo:
    }
}