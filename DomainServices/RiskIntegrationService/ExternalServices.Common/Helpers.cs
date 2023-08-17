
namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class Helpers
{
    private static string[] _kbPersonSchemas = new[] { "KBAD", "DMID" };
    const string _kbInstanceName = "KBCZ";
    const string _mpInstanceName = "MPSS";

    // bude finalne poreseno jak prekladat
    public static string GetResourceIdentifierInstanceForDealer(string? identityScheme)
        => "KBCZ";//string.IsNullOrEmpty(identityScheme) ? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.ResourceIdentifierDealerSchemeIsNull) : _kbPersonSchemas.Contains(identityScheme) ? _kbInstanceName : _mpInstanceName;

}

