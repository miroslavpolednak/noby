using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common;

public static class DefaultMockValues
{
    public const int HouseholdMainId = 1;
    public const int HouseholdCodebtorId = 2;

    public static Identity KbIdentity { get; } = new(0, IdentitySchemes.Kb);
}