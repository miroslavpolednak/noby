using CIS.Infrastructure.Data;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdHandler
    : IRequestHandler<GenerateDocumentIdMediatrRequest, Contracts.GenerateDocumentIdResponse>
{
    public async Task<Contracts.GenerateDocumentIdResponse> Handle(GenerateDocumentIdMediatrRequest request, CancellationToken cancellation)
    {
        long seq = await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<long>("SELECT NEXT VALUE FOR dbo.GenerateDocumentIdSequence", cancellation);

        return new Contracts.GenerateDocumentIdResponse
        {
            DocumentId = $"KBH{getLoginFromServiceUser()}{getEnvCode(request.Request)}{seq:D23}"
        };
    }

    private static string getEnvCode(Contracts.GenerateDocumentIdRequest request)
        => $"{request.EnvironmentName.ToString()[0]}{request.EnvironmentIndex ?? 0}";

    private string getLoginFromServiceUser()
    {
        string? serviceUser = _serviceUserAccessor.User?.Name;

        if (_configuration.ServiceUser2LoginBinding is null || !_configuration.ServiceUser2LoginBinding.Any())
            throw new CisConfigurationException(500, "ServiceUser2LoginBinding configuration is not set");

        if (_configuration.ServiceUser2LoginBinding.ContainsKey(serviceUser ?? "_default"))
            return _configuration.ServiceUser2LoginBinding[serviceUser ?? "_default"];
        else
            throw new CisConfigurationException(501, $"ServiceUser '{serviceUser}' not found in ServiceUser2LoginBinding configuration and no _default has been set");
    }

    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> _connectionProvider;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public GenerateDocumentIdHandler(
        CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> connectionProvider,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        AppConfiguration configuration)
    {
        _connectionProvider = connectionProvider;
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
    }
}
