using DomainServices.HouseholdService.Contracts;
using Google.Protobuf;
using CIS.Foms.Enums;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateIncome;

internal sealed class CreateIncomeHandler
    : IRequestHandler<CreateIncomeRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        CustomerIncomeTypes incomeType = (CustomerIncomeTypes)request.IncomeTypeId;

        // check customer existence
        if (!await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.CustomerOnSAId, cancellationToken))
            throw new CisNotFoundException(16020, "CustomerOnSA", request.CustomerOnSAId);

        // kontrola poctu prijmu
        int totalIncomesOfType = await _dbContext.CustomersIncomes
            .CountAsync(t => t.CustomerOnSAId == request.CustomerOnSAId && t.IncomeTypeId == incomeType, cancellationToken);
        if (IncomeHelpers.AlreadyHasMaxIncomes(incomeType, totalIncomesOfType))
            throw new CisValidationException(16047, "Max incomes of the type has been reached");

        var entity = new Database.Entities.CustomerOnSAIncome
        {
            CustomerOnSAId = request.CustomerOnSAId,
            Sum = request.BaseData?.Sum,
            CurrencyCode = request.BaseData?.CurrencyCode,
            IncomeSource = await getIncomeSource(request, cancellationToken),
            HasProofOfIncome = getProofOfIncomeToggle(request),
            IncomeTypeId = incomeType
        };

        var dataObject = getDataObject(request);
        if (dataObject != null)
        {
            entity.Data = Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);
            entity.DataBin = dataObject.ToByteArray();
        }

        _dbContext.CustomersIncomes.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.EntityCreated(nameof(Database.Entities.CustomerOnSAIncome), entity.CustomerOnSAIncomeId);

        return new CreateIncomeResponse
        {
            IncomeId = entity.CustomerOnSAIncomeId
        };
    }

    static bool? getProofOfIncomeToggle(CreateIncomeRequest request)
        => (CustomerIncomeTypes)request.IncomeTypeId switch
        {
            CustomerIncomeTypes.Employement => request.Employement?.HasProofOfIncome,
            _ => false
        };

    async Task<string?> getIncomeSource(CreateIncomeRequest request, CancellationToken cancellationToken)
        => (CustomerIncomeTypes)request.IncomeTypeId switch
        {
            CustomerIncomeTypes.Employement => string.IsNullOrEmpty(request.Employement?.Employer.Name) ? "-" : request.Employement?.Employer.Name,
            CustomerIncomeTypes.Enterprise => "-",
            CustomerIncomeTypes.Rent => "-",
            CustomerIncomeTypes.Other => await getOtherIncomeName(request.Other.IncomeOtherTypeId, cancellationToken),
            _ => throw new NotImplementedException("This customer income type serializer for getIncomeSource is not implemented")
        };

    async Task<string?> getOtherIncomeName(int? id, CancellationToken cancellationToken)
        => id.HasValue ? (await _codebookService.IncomeOtherTypes(cancellationToken)).FirstOrDefault(t => t.Id == id)?.Name : "-";

    static IMessage? getDataObject(CreateIncomeRequest request)
        => (CustomerIncomeTypes)request.IncomeTypeId switch
        {
            CustomerIncomeTypes.Employement => request.Employement,
            CustomerIncomeTypes.Other => request.Other,
            CustomerIncomeTypes.Enterprise => request.Entrepreneur,
            CustomerIncomeTypes.Rent => request.Rent,
            _ => throw new NotImplementedException("This customer income type serializer is not implemented")
        };

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Database.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateIncomeHandler> _logger;

    public CreateIncomeHandler(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Database.HouseholdServiceDbContext dbContext,
        ILogger<CreateIncomeHandler> logger)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
        _logger = logger;
    }
}
