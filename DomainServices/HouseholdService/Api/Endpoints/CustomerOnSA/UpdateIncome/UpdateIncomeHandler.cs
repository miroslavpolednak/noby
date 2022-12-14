using Google.Protobuf;
using CIS.Foms.Enums;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncome;

internal sealed class UpdateIncomeHandler
    : IRequestHandler<UpdateIncomeRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateIncomeRequest request, CancellationToken cancellationToken)
    {
        // entita existujiciho prijmu
        var entity = await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.IncomeId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new CisNotFoundException(16029, $"Income ID {request.IncomeId} does not exist.");

        var incomeTypeId = (CustomerIncomeTypes)request.IncomeTypeId;

        // kontrola poctu prijmu
        int totalIncomesOfType = await _dbContext.CustomersIncomes
            .CountAsync(t => t.CustomerOnSAIncomeId != request.IncomeId && t.CustomerOnSAId == entity.CustomerOnSAId && t.IncomeTypeId == incomeTypeId, cancellationToken);
        if (IncomeHelpers.AlreadyHasMaxIncomes(incomeTypeId, totalIncomesOfType))
            throw new CisValidationException(16047, "Max incomes of the type has been reached");

        var dataObject = getDataObject(incomeTypeId, request);
        entity.IncomeTypeId = incomeTypeId;
        entity.Sum = request.BaseData?.Sum;
        entity.CurrencyCode = request.BaseData?.CurrencyCode;
        entity.IncomeSource = await getIncomeSource(request, incomeTypeId, cancellationToken);
        entity.HasProofOfIncome = getProofOfIncomeToggle(request, incomeTypeId);
        entity.Data = Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);
        entity.DataBin = dataObject?.ToByteArray();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    static bool? getProofOfIncomeToggle(UpdateIncomeRequest request, CustomerIncomeTypes typeId)
        => typeId switch
        {
            CustomerIncomeTypes.Employement => request.Employement?.HasProofOfIncome,
            _ => default
        };

    async Task<string?> getIncomeSource(UpdateIncomeRequest request, CustomerIncomeTypes typeId, CancellationToken cancellationToken)
        => typeId switch
        {
            CustomerIncomeTypes.Employement => string.IsNullOrEmpty(request.Employement?.Employer.Name) ? "-" : request.Employement?.Employer.Name,
            CustomerIncomeTypes.Enterprise => "-",
            CustomerIncomeTypes.Rent => "-",
            CustomerIncomeTypes.Other => await getOtherIncomeName(request.Other.IncomeOtherTypeId, cancellationToken),
            _ => throw new NotImplementedException("This customer income type serializer for getIncomeSource is not implemented")
        };

    async Task<string?> getOtherIncomeName(int? id, CancellationToken cancellationToken)
        => id.HasValue ? (await _codebookService.IncomeOtherTypes(cancellationToken)).FirstOrDefault(t => t.Id == id)?.Name : "-";

    static IMessage getDataObject(CustomerIncomeTypes incomeType, UpdateIncomeRequest request)
        => incomeType switch
        {
            CustomerIncomeTypes.Employement => request.Employement,
            CustomerIncomeTypes.Other => request.Other,
            CustomerIncomeTypes.Enterprise => request.Entrepreneur,
            CustomerIncomeTypes.Rent => request.Rent,
            _ => throw new NotImplementedException("This customer income type serializer is not implemented")
        };

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Database.HouseholdServiceDbContext _dbContext;

    public UpdateIncomeHandler(Database.HouseholdServiceDbContext dbContext, CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}
