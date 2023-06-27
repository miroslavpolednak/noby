using AutoFixture;
using AutoFixture.AutoMoq;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Handlers.Email;
using CIS.InternalServices.NotificationService.Api.Handlers.Result;
using CIS.InternalServices.NotificationService.Api.Handlers.Sms;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;
using CIS.InternalServices.NotificationService.Api.Tests.Mocks;
using CIS.InternalServices.NotificationService.Contracts.Common;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.Testing.Common;
using Moq;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.User;

public class UserTests
{
    private readonly Fixture _fixture;
    
    public UserTests()
    {
        _fixture = FixtureFactory.Create();
        _fixture.Customize(new AutoMoqCustomization());

        _fixture.MockAppConfig();
        _fixture.MockCodebookService();
        _fixture.MockRepository();
    }

    [Fact]
    public async Task CanReadResult()
    {
        _fixture.MockUserAdapterService("UsernameA");
        
        var guid = RepositoryExtensions.SmsResultId1;
        var token = CancellationToken.None;
        var request = new GetResultRequest { NotificationId = guid };
        
        var handler = _fixture.Create<GetResultHandler>();
        var response = await handler.Handle(request, token);
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        
        mockRepository.Verify(r => r.GetResult(guid, token), Times.Once);
    }

    [Fact]
    public async Task CanSendSms()
    {
        _fixture.MockUserAdapterService("UsernameA");
        
        var token = CancellationToken.None;
        var request = new SendSmsRequest()
        {
            Text = "text",
            Type = "TypeA",
            PhoneNumber = "+420777123456"
        };
        
        var handler = _fixture.Create<SendSmsHandler>();
        var response = await handler.Handle(request, token);
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        
        mockRepository.Verify(r => r.AddResult(It.IsAny<Result>(), token), Times.Once);
        mockRepository.Verify(r => r.SaveChanges(token), Times.Once);
    }
    
    [Fact]
    public async Task CanSendSmsFromTemplate()
    {
        _fixture.MockUserAdapterService("UsernameA");
        
        var token = CancellationToken.None;
        var request = new SendSmsFromTemplateRequest
        {
            Type = "TypeC",
            PhoneNumber = "+420777123456",
            Placeholders = new List<StringKeyValuePair>
            {
                new () { Key = "var_a", Value = "value A" },
                new () { Key = "var_b", Value = "value B" }
            }
        };
        
        var handler = _fixture.Create<SendSmsFromTemplateHandler>();
        var response = await handler.Handle(request, token);
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        
        mockRepository.Verify(r => r.AddResult(It.IsAny<Result>(), token), Times.Once);
        mockRepository.Verify(r => r.SaveChanges(token), Times.Once);
    }

    [Fact]
    public async Task CanSendEmail()
    {
        _fixture.MockUserAdapterService("UsernameA");
        
        var token = CancellationToken.None;
        var request = new SendEmailRequest()
        {
            Attachments = new List<EmailAttachment>(),
            Bcc = new List<EmailAddress>(),
            Cc = new List<EmailAddress>(),
            Content = new EmailContent()
            {
                Text = "Text",
                Format = "text/html",
                Language = "cs"
            },
            From = new EmailAddress{ Value = "test@kb.cz", Party = new Party{ LegalPerson = new LegalPerson{ Name = "From" }}},
            To = new List<EmailAddress>()
            {
                new() { Value = "to@email.cz", Party = new Party{ LegalPerson = new LegalPerson{ Name = "To" }}}
            },
            Subject = "Subject"
        };
        
        var handler = _fixture.Create<SendEmailHandler>();
        var response = await handler.Handle(request, token);
        var mockRepository = _fixture.Freeze<Mock<INotificationRepository>>();
        
        mockRepository.Verify(r => r.AddResult(It.IsAny<Result>(), token), Times.Once);
        mockRepository.Verify(r => r.SaveChanges(token), Times.Once);
    }
    
    [Fact]
    public async Task CanNotReadResult()
    {
        _fixture.MockUserAdapterService("UsernameB");
        
        var guid = Guid.NewGuid();
        var token = CancellationToken.None;
        var request = new GetResultRequest { NotificationId = guid };
        
        var handler = _fixture.Create<GetResultHandler>();

        await Assert.ThrowsAsync<CisAuthorizationException>( async () =>
        {
            await handler.Handle(request, token);
        });
    }
    
    [Fact]
    public async Task CanNotSendSms()
    {
        _fixture.MockUserAdapterService("UsernameB");
        
        var token = CancellationToken.None;
        var request = new SendSmsRequest()
        {
            Text = "text",
            Type = "TypeA",
            PhoneNumber = "+420777123456"
        };
        
        var handler = _fixture.Create<SendSmsHandler>();
        await Assert.ThrowsAsync<CisAuthorizationException>( async () =>
        {
            await handler.Handle(request, token);
        });
    }
    
    [Fact]
    public async Task CanNotSendSmsFromTemplate()
    {
        _fixture.MockUserAdapterService("UsernameB");
        
        var token = CancellationToken.None;
        var request = new SendSmsFromTemplateRequest
        {
            Type = "TypeC",
            PhoneNumber = "+420777123456",
            Placeholders = new List<StringKeyValuePair>
            {
                new () { Key = "var_a", Value = "value A" },
                new () { Key = "var_b", Value = "value B" }
            }
        };
        
        var handler = _fixture.Create<SendSmsFromTemplateHandler>();
        await Assert.ThrowsAsync<CisAuthorizationException>( async () =>
        {
            await handler.Handle(request, token);
        });
    }
    
    [Fact]
    public async Task CanNotSendEmail()
    {
        _fixture.MockUserAdapterService("UsernameB");
        
        var token = CancellationToken.None;
        var request = new SendEmailRequest()
        {
            Attachments = new List<EmailAttachment>(),
            Bcc = new List<EmailAddress>(),
            Cc = new List<EmailAddress>(),
            Content = new EmailContent()
            {
                Text = "Text",
                Format = "text/html",
                Language = "cs"
            },
            From = new EmailAddress{ Value = "test@kb.cz", Party = new Party{ LegalPerson = new LegalPerson{ Name = "From" }}},
            To = new List<EmailAddress>()
            {
                new() { Value = "to@email.cz", Party = new Party{ LegalPerson = new LegalPerson{ Name = "To" }}}
            },
            Subject = "Subject"
        };
        
        var handler = _fixture.Create<SendEmailHandler>();
        await Assert.ThrowsAsync<CisAuthorizationException>( async () =>
        {
            await handler.Handle(request, token);
        });
    }
}