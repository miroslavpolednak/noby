using CIS.Core.Configuration;
using DomainServices.DocumentArchiveService.Api.Database.Repositories;
using DomainServices.DocumentArchiveService.Contracts;
using FastEnumUtility;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdHandler
    : IRequestHandler<GenerateDocumentIdRequest, Contracts.GenerateDocumentIdResponse>
{
    private readonly AppConfiguration _configuration;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;
    private readonly IDocumentSequenceRepository _documentSequenceRepository;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public GenerateDocumentIdHandler(
        IDocumentSequenceRepository documentSequenceRepository,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        AppConfiguration configuration,
        ICisEnvironmentConfiguration cisEnvironment)
    {
        _documentSequenceRepository = documentSequenceRepository;
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _cisEnvironment = cisEnvironment;
    }

    public async Task<Contracts.GenerateDocumentIdResponse> Handle(GenerateDocumentIdRequest request, CancellationToken cancellation)
    {
        var envName = request.EnvironmentName == EnvironmentNames.Unknown ? FastEnum.Parse<EnvironmentNames>(ConvertToEnvEnumStr(_cisEnvironment.EnvironmentName!))
                                                                            : request.EnvironmentName;

        long seq = await _documentSequenceRepository.GetNextDocumentSeqValue(cancellation);

        return new Contracts.GenerateDocumentIdResponse
        {
            DocumentId = $"KBH{getLoginFromServiceUser()}{getEnvCode(envName)}{seq:D23}"
        };
    }

    private static string ConvertToEnvEnumStr(string enumStr)
    {
        enumStr = enumStr.ToLower();
        if (string.IsNullOrEmpty(enumStr) || enumStr.Length < 1)
        {
            return string.Empty;
        }
        else
        {
            return char.ToUpper(enumStr[0]) + enumStr.Substring(1);
        }
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
            throw ErrorCodeMapper.CreateConfigurationException(ErrorCodeMapper.ServiceUser2LoginBindingConfigurationNotSet);

        if (_configuration.ServiceUser2LoginBinding.ContainsKey(serviceUser ?? "_default"))
            return _configuration.ServiceUser2LoginBinding[serviceUser ?? "_default"];
        else
            throw ErrorCodeMapper.CreateConfigurationException(ErrorCodeMapper.ServiceUserNotFoundInServiceUser2LoginBinding, serviceUser);
    }
}
