using AutoFixture;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using Moq;
using NSubstitute;

namespace CIS.InternalServices.NotificationService.Api.Tests.Mocks;

public static class CodebookExtensions
{
    public static void MockCodebookService(this Fixture fixture)
    {
        var mockCodebook = fixture.Freeze<Mock<ICodebookServiceClient>>();

        mockCodebook
            .Setup(m => m.SmsNotificationTypes(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<SmsNotificationTypesResponse.Types.SmsNotificationTypeItem>
            {
                new()
                {
                    Code = "TypeA",
                    McsCode = "McsCodeA",
                    Description = "DescriptionA",
                    SmsText = string.Empty,
                    IsAuditLogEnabled = false
                },
                new()
                {
                    Code = "TypeB",
                    McsCode = "McsCodeB",
                    Description = "DescriptionB",
                    SmsText = string.Empty,
                    IsAuditLogEnabled = true
                },
                new()
                {
                    Code = "TypeC",
                    McsCode = "McsCodeC",
                    Description = "DescriptionC",
                    SmsText = "Template variable A = {{var_a}} variable B = {{var_b}}",
                    IsAuditLogEnabled = false
                },
                new()
                {
                    Code = "TypeD",
                    McsCode = "McsCodeD",
                    Description = "DescriptionD",
                    SmsText = "Template variable A = {{var_a}} variable B = {{var_b}}",
                    IsAuditLogEnabled = false
                },
            }));

        mockCodebook
            .Setup(m => m.HashAlgorithms(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new List<HashAlgorithmsResponse.Types.HashAlgorithmItem>
            {
                new()
                {
                    Code = "SHA-256",
                    Description = "SHA-256"
                },
                new()
                {
                    Code = "SHA-512",
                    Description = "SHA-512"
                }
            }));
    }
}