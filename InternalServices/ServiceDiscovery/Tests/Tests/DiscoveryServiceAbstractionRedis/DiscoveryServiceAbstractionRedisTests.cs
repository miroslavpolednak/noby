using CIS.Testing;

namespace CIS.InternalServices.ServiceDiscovery.Tests;

public partial class DiscoveryServiceAbstractionRedisTests : BaseDiscoveryServiceAbstractionTest, IClassFixture<TestFixture<Program>>
{
    public DiscoveryServiceAbstractionRedisTests(TestFixture<Program> testFixture)
        : base(testFixture) { }
}
