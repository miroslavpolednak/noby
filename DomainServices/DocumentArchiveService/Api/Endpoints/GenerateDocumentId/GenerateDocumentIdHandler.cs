using CIS.Core.Configuration;
using CIS.Core.ErrorCodes;
using DomainServices.DocumentArchiveService.Api.Database.Repositories;
using DomainServices.DocumentArchiveService.Contracts;
using FastEnumUtility;
using System.Globalization;

namespace DomainServices.DocumentArchiveService.Api.Endpoints.GenerateDocumentId;

internal sealed class GenerateDocumentIdHandler
    : IRequestHandler<GenerateDocumentIdRequest, GenerateDocumentIdResponse>
{
    private readonly Configuration.AppConfiguration _configuration;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;
    private readonly IDocumentSequenceRepository _documentSequenceRepository;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public GenerateDocumentIdHandler(
        IDocumentSequenceRepository documentSequenceRepository,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Configuration.AppConfiguration configuration,
        ICisEnvironmentConfiguration cisEnvironment)
    {
        _documentSequenceRepository = documentSequenceRepository;
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _cisEnvironment = cisEnvironment;
    }

    public async Task<GenerateDocumentIdResponse> Handle(GenerateDocumentIdRequest request, CancellationToken cancellation)
    {
        var envName = request.EnvironmentName == EnvironmentNames.Unknown ? FastEnum.Parse<EnvironmentNames>(ConvertToEnvEnumStr(_cisEnvironment.EnvironmentName!))
                                                                            : request.EnvironmentName;

        long seq = await _documentSequenceRepository.GetNextDocumentSeqValue(cancellation);

        return new GenerateDocumentIdResponse
        {
            DocumentId = $"KBH{getLoginFromServiceUser()}{getEnvCode(envName)}{seq:D23}"
        };
    }

    private static string ConvertToEnvEnumStr(string enumStr)
    {
        enumStr = enumStr.ToLower(System.Globalization.CultureInfo.CurrentCulture);
        if (string.IsNullOrEmpty(enumStr) || enumStr.Length < 1)
        {
            return string.Empty;
        }
        else
        {
            return char.ToUpper(enumStr[0], CultureInfo.InvariantCulture) + enumStr[1..];
        }
    }

    private static string getEnvCode(EnvironmentNames environmentNames) => environmentNames switch
    {
        EnvironmentNames.Dev => "D0",
        EnvironmentNames.Fat => "F0",
        EnvironmentNames.Sit1 => "S1",
        EnvironmentNames.Uat => "U0",
        EnvironmentNames.Preprod => "P0",
        EnvironmentNames.Edu => "E0",
        EnvironmentNames.Quality => "Q0",
        EnvironmentNames.Prod => "00",
        EnvironmentNames.Test => "T0",
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

        if (_configuration.ServiceUser2LoginBinding is null || _configuration.ServiceUser2LoginBinding.Count == 0)
            throw ErrorCodeMapperBase.CreateConfigurationException(ErrorCodeMapper.ServiceUser2LoginBindingConfigurationNotSet);

        if (_configuration.ServiceUser2LoginBinding.ContainsKey(serviceUser ?? "_default"))
            return _configuration.ServiceUser2LoginBinding[serviceUser ?? "_default"];
        else
            throw ErrorCodeMapperBase.CreateConfigurationException(ErrorCodeMapper.ServiceUserNotFoundInServiceUser2LoginBinding, serviceUser);
    }
}
