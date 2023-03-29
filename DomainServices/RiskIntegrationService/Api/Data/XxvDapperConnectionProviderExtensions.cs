﻿using CIS.Infrastructure.Data;

namespace DomainServices.RiskIntegrationService.Api;

internal static class XxvDapperConnectionProviderExtensions
{
    const string c4mUserInfoSql = @"SELECT TOP 1 * FROM dbo.fceGetPersonHF_RIP(@id, @scheme)";

    public static async Task<ExternalServices.Dto.C4mUserInfoData> GetC4mUserInfo(
        this CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> provider,
        Contracts.Shared.Identity? identity,
        CancellationToken cancellationToken)
    {
        if (identity is null)
            throw new CisValidationException(17004, $"Can not obtain user information from XXV - identity is null");

        return await provider.ExecuteDapperRawSqlFirstOrDefault<ExternalServices.Dto.C4mUserInfoData>(c4mUserInfoSql, new { id = identity.IdentityId, scheme = identity.IdentityScheme }, cancellationToken)
            ?? throw new CisValidationException(17005, $"Can not obtain user information from XXV for {identity.IdentityId}/{identity.IdentityScheme}");
    }
}
