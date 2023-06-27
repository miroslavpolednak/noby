using AutoFixture;
using AutoFixture.AutoMoq;
using CIS.InternalServices.NotificationService.Api.Tests.Mocks;
using CIS.Testing.Common;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.SendSmsFromTemplate;

public class SendSmsFromTemplateTests
{
    private readonly Fixture _fixture;
    public SendSmsFromTemplateTests()
    {
        _fixture = FixtureFactory.Create();
        _fixture.Customize(new AutoMoqCustomization());
        
        _fixture.MockAppConfig();
        _fixture.MockCodebookService();
        _fixture.MockRepository();
        _fixture.MockUserAdapterService("UsernameA");
    }

    [Fact]
    public async Task SendSms()
    {
        // todo:
    }
}