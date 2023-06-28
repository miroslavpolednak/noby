using AutoFixture;
using CIS.InternalServices.NotificationService.Api.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace CIS.InternalServices.NotificationService.Api.Tests.Mocks;

public static class AppConfigurationExtensions
{
    public static void MockAppConfig(this Fixture fixture)
    {
        var mockAppConfiguration = fixture.Freeze<Mock<IOptions<AppConfiguration>>>();
        mockAppConfiguration
            .Setup(m => m.Value)
            .Returns(new AppConfiguration
            {
                Consumers = new List<Consumer>
                {
                    new() 
                    {
                        ConsumerId = "ConsumerA",
                        Username = "UsernameA",
                        CanReadResult = true,
                        CanSendEmail = true,
                        CanSendSms = true 
                    },
                    new() 
                    {
                        ConsumerId = "ConsumerB",
                        Username = "UsernameB",
                        CanReadResult = false,
                        CanSendEmail = false,
                        CanSendSms = false 
                    }
                },
                EmailFormats = new HashSet<string> { "text/html" },
                EmailSenders = new EmailSenders
                {
                    Mcs = new HashSet<string> { "kb.cz" },
                    Mpss = new HashSet<string> { "mpss.cz" }
                },
                KafkaTopics = new KafkaTopics()
                {
                    McsResult = "mcs-result",
                    McsSender = "mcs-sender",
                    NobySendEmail = "noby-email"
                },
                S3Buckets = new S3Buckets
                {
                    Mcs = "mcs-bucket",
                    Mpss = "mpss-bucket"
                },
                EmailLanguageCodes = new HashSet<string> { "cs" }
            });
    }
}