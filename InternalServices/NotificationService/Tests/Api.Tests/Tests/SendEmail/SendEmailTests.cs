using AutoFixture;
using CIS.Testing.Common;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.SendEmail;

public class SendEmailTests
{
    private readonly Fixture _fixture;
    
    public SendEmailTests()
    {
        _fixture = FixtureFactory.Create();
    }
}