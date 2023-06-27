using AutoFixture;
using CIS.Testing.Common;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.SendSms;

public class SendSmsTests
{
    private readonly Fixture _fixture;
    
    public SendSmsTests()
    {
        _fixture = FixtureFactory.Create();
    }
}