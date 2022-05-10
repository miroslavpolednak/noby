using DomainServices.RiskIntegrationService.Api.Shared;
using CIS.Infrastructure.Data;

namespace Mpss.Rip.Infrastructure.Services.PersonDealer;

/// <summary>
/// Vrátí detaily Uživatele (Person / Dealer)
/// </summary>
[CIS.Infrastructure.Attributes.ScopedService]
internal class PersonDealerService : IPersonDealerService
{
    public const string PersonKbIdentityScheme = "KBAD";
    public const string PersonMpIdentityScheme = "MPAD";

    private readonly CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<PersonDealerService> _logger;

    public PersonDealerService(CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> connectionProvider, ILogger<PersonDealerService> logger)
    {
        _connectionProvider = connectionProvider;
        _logger = logger;
    }

    /// <summary>
    /// Vrátí detailní data o uživateli (Person/Dealer)
    /// </summary>
    /// <param name="identity"></param>
    /// <param name="identityScheme"></param>
    public async Task<PersonDealerExtension> GetUserData(string identity, string identityScheme)
    {
        var user = (await _connectionProvider.ExecuteDapperRawSqlToList<PersonDealerExtension>("SELECT * FROM dbo.fceGetPersonHF_RIP(@identity, @identityScheme)", new { identity = identity, identityScheme = identityScheme }))
            ?.FirstOrDefault();

        if (identityScheme.Equals(PersonKbIdentityScheme) || identityScheme.Equals(PersonMpIdentityScheme))
        {
            user.IsDealer = false;
        }
        else
        {
            user.IsDealer = true;
        }

        return user;
    }
}
