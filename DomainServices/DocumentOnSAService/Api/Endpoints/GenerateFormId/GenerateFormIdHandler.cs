using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GenerateFormId;

public class GenerateFormIdHandler : IRequestHandler<GenerateFormIdRequest, GenerateFormIdResponse>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;

    private const short MaxVersion = 99;
    private const string DefaultSystem = "N";

    public GenerateFormIdHandler(DocumentOnSAServiceDbContext dbContext)
    {
        _dbContext = dbContext;
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
            version = MaxVersion;
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
            version = UpdateVersionOfFormId(generatedFormId, false, MaxVersion);
            sequenceId = generatedFormId.Id;
        }
        // Exist with max version (99)
        else if (generatedFormId is not null && generatedFormId.Version >= MaxVersion && !request.IsFormIdFinal)
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

    private static string CreateFormId(long sequenceId, short version)
    {
        if (version > MaxVersion)
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.VersionHaveToBeLowerThanMaxVersion, MaxVersion);

        var identifier = sequenceId * 100 + version;

        return $"{DefaultSystem}{identifier:D14}";
    }

    private static short UpdateVersionOfFormId(GeneratedFormId generatedFormId, bool increaseVersion, short version = 0)
    {
        if (increaseVersion)
        {
            generatedFormId.Version += 1;
        }
        else
        {
            generatedFormId.Version = version == 0 && version != MaxVersion ? throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.SetVersionDirectlyError, MaxVersion) : version;
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
}
