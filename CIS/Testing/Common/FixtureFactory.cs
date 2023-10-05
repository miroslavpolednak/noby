using AutoFixture;
using SharedTypes.GrpcTypes;

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