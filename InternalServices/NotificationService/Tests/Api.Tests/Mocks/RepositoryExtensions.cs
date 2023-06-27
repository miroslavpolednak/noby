using AutoFixture;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using Moq;
using Result = CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction.Result;

namespace CIS.InternalServices.NotificationService.Api.Tests.Mocks;

public static class RepositoryExtensions
{
    public static Guid SmsResultId1 = Guid.NewGuid();
    public static Guid EmailResultId1 = Guid.NewGuid();

    public static Guid SmsResultIdForAdding = Guid.NewGuid();
    public static Guid EmailResultIdForAdding = Guid.NewGuid();
    
    public static SmsResult SmsResult1 = new()
    {
        Id = SmsResultId1,
        Channel = NotificationChannel.Sms,
        ErrorSet = new HashSet<ResultError>(),
        CustomId = "CustomIdA",
        Type = "TypeA",
        State = NotificationState.InProgress,
        CountryCode = "+420",
        PhoneNumber = "777123456",
        CreatedBy = "UsernameA",
        RequestTimestamp = new DateTime(2023, 1, 1, 1, 0, 0)
    };
    
    public static EmailResult EmailResult1 = new()
    {
        Id = EmailResultId1,
        Channel = NotificationChannel.Sms,
        ErrorSet = new HashSet<ResultError>(),
        CustomId = "CustomIdA",
        State = NotificationState.InProgress,
        CreatedBy = "UsernameA",
        RequestTimestamp = new DateTime(2023, 1, 1, 1, 1, 0)
    };
    
    public static void MockRepository(this Fixture fixture)
    {
        var mockRepository = fixture.Freeze<Mock<INotificationRepository>>();
        mockRepository
            .Setup(r => r.GetResult(SmsResultId1, It.IsAny<CancellationToken>()))
            .Returns((Guid id, CancellationToken token) => Task.FromResult<Result>(SmsResult1));
        
        mockRepository
            .Setup(r => r.GetResult(EmailResultId1, It.IsAny<CancellationToken>()))
            .Returns((Guid id, CancellationToken token) => Task.FromResult<Result>(EmailResult1));

        mockRepository
            .Setup(r => r.GetResult(It.IsNotIn(SmsResultId1, EmailResultId1), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CisNotFoundException(0, "Result not found."));

        mockRepository
            .Setup(r => r.NewSmsResult())
            .Returns(new SmsResult
            {
                Id = SmsResultIdForAdding,
                Channel = NotificationChannel.Sms,
                State = NotificationState.InProgress,
                ResultTimestamp = null,
                ErrorSet = new HashSet<ResultError>()
            });

        mockRepository
            .Setup(r => r.NewEmailResult())
            .Returns(new EmailResult
            {
                Id = EmailResultIdForAdding,
                Channel = NotificationChannel.Email,
                State = NotificationState.InProgress,
                ResultTimestamp = null,
                ErrorSet = new HashSet<ResultError>(),
            });
    }
}