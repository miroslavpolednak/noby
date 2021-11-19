using CIS.Testing;

namespace CIS.InternalServices.ServiceDiscovery.Tests;

public partial class DiscoveryServiceAbstractionInMemoryTests : BaseDiscoveryServiceAbstractionTest, IClassFixture<TestFixture<Program>>
{
    public DiscoveryServiceAbstractionInMemoryTests(TestFixture<Program> testFixture)
        : base(testFixture) { }
}
