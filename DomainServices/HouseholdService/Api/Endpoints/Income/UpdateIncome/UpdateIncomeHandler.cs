using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.UpdateIncome;

internal sealed class UpdateIncomeHandler
    : IRequestHandler<UpdateIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        // entita existujiciho prijmu
        var entity = await _dbContext
            .CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.IncomeId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.IncomeNotFound, request.IncomeId);

        var incomeTypeId = (CustomerIncomeTypes)request.IncomeTypeId;

        // kontrola poctu prijmu
        int totalIncomesOfType = await _dbContext
            .CustomersIncomes
            .CountAsync(t => t.CustomerOnSAIncomeId != request.IncomeId && t.CustomerOnSAId == entity.CustomerOnSAId && t.IncomeTypeId == incomeTypeId, cancellationToken);

        if (IncomeHelpers.AlreadyHasMaxIncomes(incomeTypeId, totalIncomesOfType))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.MaxIncomesReached);
        }

        entity.IncomeTypeId = incomeTypeId;
        entity.Sum = request.BaseData?.Sum;
        entity.CurrencyCode = request.BaseData?.CurrencyCode;
        entity.IncomeSource = await getIncomeSource(request, incomeTypeId, cancellationToken);
        entity.HasProofOfIncome = getProofOfIncomeToggle(request, incomeTypeId);

        await _dbContext.SaveChangesAsync(cancellationToken);

        switch ((CustomerIncomeTypes)request.IncomeTypeId)
        {
            case CustomerIncomeTypes.Employement:
                await _documentDataStorage.InsertOrUpdateDataWithMapper<Database.DocumentDataEntities.IncomeEmployement, IncomeDataEmployement>(request.Employement, entity.CustomerOnSAIncomeId, true, cancellationToken);
                break;

            case CustomerIncomeTypes.Entrepreneur:
                await _documentDataStorage.InsertOrUpdateDataWithMapper<Database.DocumentDataEntities.IncomeEntrepreneur, IncomeDataEntrepreneur>(request.Entrepreneur, entity.CustomerOnSAIncomeId, true, cancellationToken);
                break;

            case CustomerIncomeTypes.Other:
                await _documentDataStorage.InsertOrUpdateDataWithMapper<Database.DocumentDataEntities.IncomeOther, IncomeDataOther>(request.Other, entity.CustomerOnSAIncomeId, true, cancellationToken);
                break;
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private static bool? getProofOfIncomeToggle(UpdateIncomeRequest request, CustomerIncomeTypes typeId)
        => typeId switch
        {
            CustomerIncomeTypes.Employement => request.Employement?.HasProofOfIncome,
            _ => default
        };

    private async Task<string?> getIncomeSource(UpdateIncomeRequest request, CustomerIncomeTypes typeId, CancellationToken cancellationToken)
        => typeId switch
        {
            CustomerIncomeTypes.Employement => string.IsNullOrEmpty(request.Employement?.Employer.Name) ? "-" : request.Employement?.Employer.Name,
            CustomerIncomeTypes.Other => await getOtherIncomeName(request.Other.IncomeOtherTypeId, cancellationToken),
            _ => "-"
        };

    private async Task<string?> getOtherIncomeName(int? id, CancellationToken cancellationToken)
        => id.HasValue ? (await _codebookService.IncomeOtherTypes(cancellationToken)).FirstOrDefault(t => t.Id == id)?.Name : "-";

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ICodebookServiceClient _codebookService;
    private readonly Database.HouseholdServiceDbContext _dbContext;

    public UpdateIncomeHandler(Database.HouseholdServiceDbContext dbContext, ICodebookServiceClient codebookService, IDocumentDataStorage documentDataStorage)
    {
        _documentDataStorage = documentDataStorage;
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}