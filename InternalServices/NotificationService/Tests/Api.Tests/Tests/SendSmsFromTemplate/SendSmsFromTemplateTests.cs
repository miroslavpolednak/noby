using AutoFixture;
using CIS.Testing.Common;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.SendSmsFromTemplate;

public class SendSmsFromTemplateTests
{
    private readonly Fixture _fixture;
    
    public SendSmsFromTemplateTests()
    {
        _fixture = FixtureFactory.Create();
    }
}