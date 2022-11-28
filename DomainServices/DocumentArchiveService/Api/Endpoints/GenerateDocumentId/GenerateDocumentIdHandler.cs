using CIS.Infrastructure.Data;
using DomainServices.DocumentArchiveService.Contracts;
using FastEnumUtility;
using Microsoft.Extensions.Options;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdHandler
    : IRequestHandler<GenerateDocumentIdMediatrRequest, Contracts.GenerateDocumentIdResponse>
{
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> _connectionProvider;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public GenerateDocumentIdHandler(
        CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> connectionProvider,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        IOptions<AppConfiguration> configuration)
    {
        _connectionProvider = connectionProvider;
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration?.Value ?? throw new ArgumentNullException();
    }

    public async Task<Contracts.GenerateDocumentIdResponse> Handle(GenerateDocumentIdMediatrRequest request, CancellationToken cancellation)
    {
        long seq = await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<long>("SELECT NEXT VALUE FOR dbo.GenerateDocumentIdSequence", cancellation);

        return new Contracts.GenerateDocumentIdResponse
        {
            DocumentId = $"KBH{getLoginFromServiceUser()}{getEnvCode(request.Request.EnvironmentName)}{seq:D23}"
        };
    }

    private static string getEnvCode(Contracts.EnvironmentNames environmentNames) => environmentNames switch
    {
        EnvironmentNames.Dev => "D",
        EnvironmentNames.Fat => "F",
        EnvironmentNames.Sit => "S",
        EnvironmentNames.Uat => "U",
        EnvironmentNames.Preprod => "P",
        EnvironmentNames.Edu => "E",
        EnvironmentNames.Prod => "R",
        EnvironmentNames.Unknown => HandleUnsupportedEnv(environmentNames),
        _ => HandleUnsupportedEnv(environmentNames)
    };

    private static string HandleUnsupportedEnv(EnvironmentNames environmentNames)
    {
        throw new ArgumentException($"Unsupported kind of environment {environmentNames.FastToString()}");
    }

    private string getLoginFromServiceUser()
    {
        string? serviceUser = _serviceUserAccessor.User?.Name;

        if (_configuration.ServiceUser2LoginBinding is null || !_configuration.ServiceUser2LoginBinding.Any())
            throw new CisConfigurationException(14012, "ServiceUser2LoginBinding configuration is not set");

        if (_configuration.ServiceUser2LoginBinding.ContainsKey(serviceUser ?? "_default"))
            return _configuration.ServiceUser2LoginBinding[serviceUser ?? "_default"];
        else
            throw new CisConfigurationException(14013, $"ServiceUser '{serviceUser}' not found in ServiceUser2LoginBinding configuration and no _default has been set");
    }
}
