using CIS.Testing;

namespace CIS.InternalServices.ServiceDiscovery.Tests;

public partial class DiscoveryServiceInMemoryTests : BaseDiscoveryServiceTest, IClassFixture<TestFixture<Program>>
{
    public DiscoveryServiceInMemoryTests(TestFixture<Program> testFixture) : base(testFixture) { }
}
