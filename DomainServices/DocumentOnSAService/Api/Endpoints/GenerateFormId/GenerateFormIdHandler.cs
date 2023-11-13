using CIS.Core.Configuration;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using FastEnumUtility;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GenerateFormId;

public class GenerateFormIdHandler : IRequestHandler<GenerateFormIdRequest, GenerateFormIdResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;
    private const short _maxVersion = 99;
    private const string _defaultSystem = "N";

    public GenerateFormIdHandler(
        DocumentOnSAServiceDbContext dbContext,
        ICisEnvironmentConfiguration cisEnvironment)
    {
        _dbContext = dbContext;
        _cisEnvironment = cisEnvironment;
    }

    public async Task<GenerateFormIdResponse> Handle(GenerateFormIdRequest request, CancellationToken cancellationToken)
    {
        var generatedFormId = request.HouseholdId is null ? null : await _dbContext.GeneratedFormId.OrderByDescending(e => e.Id)
             .FirstOrDefaultAsync(e => e.HouseholdId == request.HouseholdId, cancellationToken);

        short version = 0;
        long sequenceId = 0;

        // New with final version
        if (generatedFormId is null && request.IsFormIdFinal)
        {
            version = _maxVersion;
            sequenceId = await CreateNewFormId(request, version, cancellationToken);
        }
        // New with init version
        else if (generatedFormId is null && !request.IsFormIdFinal)
        {
            version = 1;
            sequenceId = await CreateNewFormId(request, version, cancellationToken);
        }
        // Exist with request to final version
        else if (generatedFormId is not null && request.IsFormIdFinal)
        {
            version = UpdateVersionOfFormId(generatedFormId, false, _maxVersion);
            sequenceId = generatedFormId.Id;
        }
        // Exist with max version (99)
        else if (generatedFormId is not null && generatedFormId.Version >= _maxVersion && !request.IsFormIdFinal)
        {
            //Generate new record in table (new sequence[id]) and set version to 1
            version = 1;
            sequenceId = await CreateNewFormId(request, version, cancellationToken);
        }
        // Exist increase version
        else if (generatedFormId is not null && !request.IsFormIdFinal)
        {
            version = UpdateVersionOfFormId(generatedFormId, true, version);
            sequenceId = generatedFormId.Id;
        }
        else
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CombinationOfParametersNotSupported);
        }

        if (_dbContext.ChangeTracker.HasChanges())
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return new GenerateFormIdResponse
        {
            FormId = CreateFormId(sequenceId, version)
        };
    }

    private string CreateFormId(long sequenceId, short version)
    {
        if (version > _maxVersion)
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.VersionHaveToBeLowerThanMaxVersion, _maxVersion);

        var identifier = (sequenceId * 100) + version;

        var envName = FastEnum.Parse<EnvironmentNames>(ConvertToEnvEnumStr(_cisEnvironment.EnvironmentName!));

        return $"{_defaultSystem}{GetEnvCode(envName)}{identifier:D12}";
    }

    private static short UpdateVersionOfFormId(GeneratedFormId generatedFormId, bool increaseVersion, short version = 0)
    {
        if (increaseVersion)
        {
            generatedFormId.Version++;
        }
        else
        {
            generatedFormId.Version = version == 0 && version != _maxVersion ? throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.SetVersionDirectlyError, _maxVersion) : version;
        }

        return generatedFormId.Version;
    }

    private async Task<long> CreateNewFormId(GenerateFormIdRequest request, short version, CancellationToken cancellationToken)
    {
        var entity = new GeneratedFormId
        {
            HouseholdId = request.HouseholdId,
            Version = version,
            IsFormIdFinal = request.IsFormIdFinal
        };

        await _dbContext.GeneratedFormId.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    private static string GetEnvCode(EnvironmentNames environmentNames) => environmentNames switch
    {
        EnvironmentNames.Dev => "D0",
        EnvironmentNames.Fat => "F0",
        EnvironmentNames.Sit1 => "S1",
        EnvironmentNames.Uat => "U0",
        EnvironmentNames.Preprod => "P0",
        EnvironmentNames.Edu => "E0",
        EnvironmentNames.Prod => "00",
        EnvironmentNames.Test => "T0",
        EnvironmentNames.Unknown => HandleUnsupportedEnv(environmentNames),
        _ => HandleUnsupportedEnv(environmentNames)
    };

    private static string HandleUnsupportedEnv(EnvironmentNames environmentNames)
    {
        throw new ArgumentException($"Unsupported kind of environment {environmentNames.FastToString()}");
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
            return char.ToUpper(enumStr[0]) + enumStr[1..];
        }
    }
}
