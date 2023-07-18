using CIS.Infrastructure.Data;

namespace DomainServices.RiskIntegrationService.Api;

internal static class XxvDapperConnectionProviderExtensions
{
    const string c4mUserInfoSql = @"SELECT TOP 1 * FROM dbo.fceGetPersonHF_RIP(@id, @scheme)";

    public static async Task<ExternalServices.Dto.C4mUserInfoData?> GetC4mUserInfo(
        this CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> provider,
        Contracts.Shared.Identity? identity,
        CancellationToken cancellationToken)
    {
        if (identity is null)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UserIdentityIsNull);

        return await provider.ExecuteDapperRawSqlFirstOrDefaultAsync<ExternalServices.Dto.C4mUserInfoData>(c4mUserInfoSql, new { id = identity.IdentityId, scheme = identity.IdentityScheme }, cancellationToken);
            //?? ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.UserInfoIsNull, $"{identity.IdentityId}/{identity.IdentityScheme}");
    }
}
