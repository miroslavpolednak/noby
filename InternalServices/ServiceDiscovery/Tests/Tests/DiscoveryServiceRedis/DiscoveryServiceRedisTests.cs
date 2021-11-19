using CIS.Testing;

namespace CIS.InternalServices.ServiceDiscovery.Tests;

public partial class DiscoveryServiceRedisTests : BaseDiscoveryServiceTest, IClassFixture<TestFixture<Program>>
{
    public DiscoveryServiceRedisTests(TestFixture<Program> testFixture) : base(testFixture) { }
}
