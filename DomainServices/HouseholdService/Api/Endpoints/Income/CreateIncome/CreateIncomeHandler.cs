using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Endpoints.Income.CreateIncome;

internal sealed class CreateIncomeHandler
    : IRequestHandler<CreateIncomeRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var entity = new Database.Entities.CustomerOnSAIncome
        {
            CustomerOnSAId = request.CustomerOnSAId,
            Sum = request.BaseData?.Sum,
            CurrencyCode = request.BaseData?.CurrencyCode,
            IncomeSource = await getIncomeSource(request, cancellationToken),
            HasProofOfIncome = getProofOfIncomeToggle(request),
            IncomeTypeId = (CustomerIncomeTypes)request.IncomeTypeId
        };
        _dbContext.CustomersIncomes.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        switch ((CustomerIncomeTypes)request.IncomeTypeId)
        {
            case CustomerIncomeTypes.Employement:
                await _documentDataStorage.InsertOrUpdateDataWithMapper<Database.DocumentDataEntities.IncomeEmployement, IncomeDataEmployement>(request.Employement, entity.CustomerOnSAIncomeId, cancellationToken: cancellationToken);
                break;

            case CustomerIncomeTypes.Entrepreneur:
                await _documentDataStorage.InsertOrUpdateDataWithMapper<Database.DocumentDataEntities.IncomeEntrepreneur, IncomeDataEntrepreneur>(request.Entrepreneur, entity.CustomerOnSAIncomeId, cancellationToken: cancellationToken);
                break;

            case CustomerIncomeTypes.Other:
                await _documentDataStorage.InsertOrUpdateDataWithMapper<Database.DocumentDataEntities.IncomeOther, IncomeDataOther>(request.Other, entity.CustomerOnSAIncomeId, cancellationToken: cancellationToken);
                break;
        }

        _logger.EntityCreated(nameof(Database.Entities.CustomerOnSAIncome), entity.CustomerOnSAIncomeId);

        return new CreateIncomeResponse
        {
            IncomeId = entity.CustomerOnSAIncomeId
        };
    }

    private static bool? getProofOfIncomeToggle(CreateIncomeRequest request)
        => (CustomerIncomeTypes)request.IncomeTypeId switch
        {
            CustomerIncomeTypes.Employement => request.Employement?.HasProofOfIncome,
            _ => false
        };

    private async Task<string?> getIncomeSource(CreateIncomeRequest request, CancellationToken cancellationToken)
        => (CustomerIncomeTypes)request.IncomeTypeId switch
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
    private readonly ILogger<CreateIncomeHandler> _logger;

    public CreateIncomeHandler(
        IDocumentDataStorage documentDataStorage,
        ICodebookServiceClient codebookService,
        Database.HouseholdServiceDbContext dbContext,
        ILogger<CreateIncomeHandler> logger)
    {
        _documentDataStorage = documentDataStorage;
        _codebookService = codebookService;
        _dbContext = dbContext;
        _logger = logger;
    }
}