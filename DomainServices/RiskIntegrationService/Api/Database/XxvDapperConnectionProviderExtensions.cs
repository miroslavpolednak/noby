using CIS.Infrastructure.Data;

namespace DomainServices.RiskIntegrationService.Api;

internal static class XxvDapperConnectionProviderExtensions
{
    const string c4mUserInfoSql = @"SELECT TOP 1 * FROM dbo.fceGetPersonHF_RIP(@id, @scheme)";

    public static async Task<Dto.C4mUserInfoData> GetC4mUserInfo(
        this CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> provider,
        string? identityId,
        string? identityScheme,
        CancellationToken cancellationToken)
        => await provider.ExecuteDapperRawSqlFirstOrDefault<Dto.C4mUserInfoData>(c4mUserInfoSql, new { id = identityId, scheme = identityScheme }, cancellationToken)
        ?? throw new CisValidationException(0, $"Can not obtain user information from XXV for {identityId}/{identityScheme}");
}
