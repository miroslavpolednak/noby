using AutoFixture;
using CIS.Testing.Common;

namespace CIS.InternalServices.NotificationService.Api.Tests.Tests.GetResult;

public class GetResultTests
{
    private readonly Fixture _fixture;
    
    public GetResultTests()
    {
        _fixture = FixtureFactory.Create();
    }
}