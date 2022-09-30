using DomainServices.HouseholdService.Contracts;
using Google.Protobuf;
using CIS.Foms.Enums;

namespace DomainServices.HouseholdService.Api.Handlers.CustomerOnSA.CreateIncome;

internal class CreateIncomeHandler
    : IRequestHandler<CreateIncomeMediatrRequest, CreateIncomeResponse>
{
    public async Task<CreateIncomeResponse> Handle(CreateIncomeMediatrRequest request, CancellationToken cancellation)
    {
        CustomerIncomeTypes incomeType = (CustomerIncomeTypes)request.Request.IncomeTypeId;

        // check customer existence
        if (!await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId, cancellation))
            throw new CisNotFoundException(16020, "CustomerOnSA", request.Request.CustomerOnSAId);

        // kontrola poctu prijmu
        int totalIncomesOfType = await _dbContext.CustomersIncomes
            .CountAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId && t.IncomeTypeId == incomeType, cancellation);
        if (alreadyMaxIncomes(incomeType, totalIncomesOfType))
            throw new CisValidationException(16047, "Max incomes of the type has been reached");

        var entity = new Repositories.Entities.CustomerOnSAIncome
        {
            CustomerOnSAId = request.Request.CustomerOnSAId,
            Sum = request.Request.BaseData?.Sum,
            CurrencyCode = request.Request.BaseData?.CurrencyCode,
            IncomeSource = await getIncomeSource(request.Request, cancellation),
            HasProofOfIncome = getProofOfIncomeToggle(request.Request),
            IncomeTypeId = incomeType
        };

        var dataObject = getDataObject(request.Request);
        if (dataObject != null)
        {
            entity.Data = Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);
            entity.DataBin = dataObject.ToByteArray();
        }

        _dbContext.CustomersIncomes.Add(entity);
        await _dbContext.SaveChangesAsync(cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.CustomerOnSAIncome), entity.CustomerOnSAIncomeId);

        return new CreateIncomeResponse
        {
            IncomeId = entity.CustomerOnSAIncomeId
        };
    }

    static bool alreadyMaxIncomes(CustomerIncomeTypes incomeType, int count)
        => incomeType switch
        {
            CustomerIncomeTypes.Employement => count >= 3,
            CustomerIncomeTypes.Enterprise => count >= 1,
            CustomerIncomeTypes.Rent => count >= 1,
            CustomerIncomeTypes.Other => count >= 10,
            _ => throw new NotImplementedException("This customer income type count check is not implemented")
        };

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

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.HouseholdServiceDbContext _dbContext;
    private readonly ILogger<CreateIncomeHandler> _logger;

    public CreateIncomeHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.HouseholdServiceDbContext dbContext,
        ILogger<CreateIncomeHandler> logger)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
        _logger = logger;
    }
}
