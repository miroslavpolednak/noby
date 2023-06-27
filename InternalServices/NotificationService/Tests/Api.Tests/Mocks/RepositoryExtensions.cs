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
    public static Guid SmsResultId2 = Guid.NewGuid();
    public static Guid SmsResultId3 = Guid.NewGuid();
    public static Guid EmailResultId1 = Guid.NewGuid();
    public static Guid EmailResultId2 = Guid.NewGuid();
    public static Guid EmailResultId3 = Guid.NewGuid();

    public static Guid SmsResultIdForAdding = Guid.NewGuid();
    public static Guid EmailResultIdForAdding = Guid.NewGuid();

    public static HashSet<Result> Results = new()
    {
        new SmsResult
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
        },
        new SmsResult
        {
            Id = SmsResultId2,
            Channel = NotificationChannel.Sms,
            ErrorSet = new HashSet<ResultError>(),
            DocumentId = "DocumentIdA",
            Type = "TypeB",
            State = NotificationState.InProgress,
            CountryCode = "+420",
            PhoneNumber = "777123456",
            CreatedBy = "UsernameA",
            RequestTimestamp = new DateTime(2023, 1, 1, 1, 0, 0)
        },
        new SmsResult
        {
            Id = SmsResultId3,
            Channel = NotificationChannel.Sms,
            ErrorSet = new HashSet<ResultError>(),
            Identity = "IdentityA",
            IdentityScheme = "IdentitySchemeA",
            Type = "TypeC",
            State = NotificationState.InProgress,
            CountryCode = "+420",
            PhoneNumber = "777123456",
            CreatedBy = "UsernameA",
            RequestTimestamp = new DateTime(2023, 1, 1, 1, 0, 0)
        },
        new EmailResult
        {
            Id = EmailResultId1,
            Channel = NotificationChannel.Email,
            ErrorSet = new HashSet<ResultError>(),
            CustomId = "CustomIdA",
            State = NotificationState.InProgress,
            CreatedBy = "UsernameA",
            RequestTimestamp = new DateTime(2023, 1, 1, 1, 1, 0)
        },
        new EmailResult
        {
            Id = EmailResultId2,
            Channel = NotificationChannel.Email,
            ErrorSet = new HashSet<ResultError>(),
            DocumentId = "DocumentIdA",
            State = NotificationState.InProgress,
            CreatedBy = "UsernameA",
            RequestTimestamp = new DateTime(2023, 1, 1, 1, 1, 0)
        },
        new EmailResult
        {
            Id = EmailResultId3,
            Channel = NotificationChannel.Email,
            ErrorSet = new HashSet<ResultError>(),
            Identity = "IdentityA",
            IdentityScheme = "IdentitySchemeA",
            State = NotificationState.InProgress,
            CreatedBy = "UsernameA",
            RequestTimestamp = new DateTime(2023, 1, 1, 1, 1, 0)
        }
    };
    
    public static void MockRepository(this Fixture fixture)
    {
        var mockRepository = fixture.Freeze<Mock<INotificationRepository>>();

        mockRepository
            .Setup(r => r.GetResult(
                It.IsIn(Results.Select(result => result.Id)),
                It.IsAny<CancellationToken>()))
            .Returns((Guid id, CancellationToken token) => Task.FromResult(Results.Single(r => r.Id == id)));
        
        mockRepository
            .Setup(r => r.GetResult(
                It.IsNotIn(Results.Select(result => result.Id)),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CisNotFoundException(0, "Result not found."));

        mockRepository
            .Setup(r => r.SearchResultsBy(
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>()))
            .Returns((string? identity, string? identityScheme, string? customId, string? documentId) =>
                Task.FromResult(Results
                    .Where(r => string.IsNullOrEmpty(identity) || r.Identity == identity)
                    .Where(r => string.IsNullOrEmpty(identityScheme) || r.IdentityScheme == identityScheme)
                    .Where(r => string.IsNullOrEmpty(customId) || r.CustomId == customId)
                    .Where(r => string.IsNullOrEmpty(documentId) || r.DocumentId == documentId)));
        
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