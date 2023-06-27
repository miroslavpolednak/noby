using AutoFixture;
using CIS.Testing.Common;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.ConsumeSendEmail;

public class ConsumeSendEmailTests
{
    private readonly Fixture _fixture;
    
    public ConsumeSendEmailTests()
    {
        _fixture = FixtureFactory.Create();
    }
}