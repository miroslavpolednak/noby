using AutoFixture;
using CIS.Infrastructure.gRPC.CisTypes;

namespace CIS.Testing.Common;

public static class FixtureFactory
{
    public static Fixture Create()
    {
        var fixture = new Fixture();

        fixture.Inject((GrpcDate)DateTime.Now);
        fixture.Inject<NullableGrpcDate>(null!);

        return fixture;
    }
}