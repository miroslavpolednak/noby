using SharedTypes.GrpcTypes;

namespace SharedTypes.Extensions;

public static class IdentityExtensions
{

    public static bool HasKbIdentity(this IEnumerable<Identity> identities) =>
        identities.Any(KbIdentityCondition);

    public static bool HasMpIdentity(this IEnumerable<Identity> identities) =>
        identities.Any(MpIdentityCondition);

    public static Identity GetKbIdentity(this IEnumerable<Identity> identities) =>
        identities.GetKbIdentityOrDefault() ?? throw new InvalidOperationException("Missing KB identity");

    public static Identity? GetKbIdentityOrDefault(this IEnumerable<Identity> identities) =>
        identities.SingleOrDefault(KbIdentityCondition);

    public static Identity GetMpIdentity(this IEnumerable<Identity> identities) =>
        identities.GetMpIdentityOrDefault() ?? throw new InvalidOperationException("Missing MP identity");

    public static Identity? GetMpIdentityOrDefault(this IEnumerable<Identity> identities) =>
        identities.SingleOrDefault(MpIdentityCondition);

    public static Identity GetIdentity(this IEnumerable<Identity> identities, Identity.Types.IdentitySchemes preferredScheme) =>
        identities.SingleOrDefault(t => t.IdentityScheme == preferredScheme, identities.First());


    private static bool KbIdentityCondition(Identity identity) => identity is { IdentityScheme: Identity.Types.IdentitySchemes.Kb, IdentityId: > 0 };

    private static bool MpIdentityCondition(Identity identity) => identity is { IdentityScheme: Identity.Types.IdentitySchemes.Mp, IdentityId: > 0 };

}