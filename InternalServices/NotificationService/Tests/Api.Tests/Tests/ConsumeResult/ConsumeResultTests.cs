using AutoFixture;
using CIS.Testing.Common;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.ConsumeResult;

public class ConsumeResultTests
{
    private readonly Fixture _fixture;
    
    public ConsumeResultTests()
    {
        _fixture = FixtureFactory.Create();
    }
}