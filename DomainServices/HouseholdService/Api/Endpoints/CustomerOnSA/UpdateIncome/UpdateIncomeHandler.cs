using _SA = DomainServices.HouseholdService.Contracts;
using Google.Protobuf;
using CIS.Foms.Enums;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateIncome;

internal sealed class UpdateIncomeHandler
    : IRequestHandler<UpdateIncomeMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(UpdateIncomeMediatrRequest request, CancellationToken cancellation)
    {
        // entita existujiciho prijmu
        var entity = await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.Request.IncomeId)
            .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(16029, $"Income ID {request.Request.IncomeId} does not exist.");

        var incomeTypeId = (CustomerIncomeTypes)request.Request.IncomeTypeId;

        // kontrola poctu prijmu
        int totalIncomesOfType = await _dbContext.CustomersIncomes
            .CountAsync(t => t.CustomerOnSAIncomeId != request.Request.IncomeId && t.CustomerOnSAId == entity.CustomerOnSAId && t.IncomeTypeId == incomeTypeId, cancellation);
        if (IncomeHelpers.AlreadyHasMaxIncomes(incomeTypeId, totalIncomesOfType))
            throw new CisValidationException(16047, "Max incomes of the type has been reached");

        var dataObject = getDataObject(incomeTypeId, request.Request);
        entity.IncomeTypeId = incomeTypeId;
        entity.Sum = request.Request.BaseData?.Sum;
        entity.CurrencyCode = request.Request.BaseData?.CurrencyCode;
        entity.IncomeSource = await getIncomeSource(request.Request, incomeTypeId, cancellation);
        entity.HasProofOfIncome = getProofOfIncomeToggle(request.Request, incomeTypeId);
        entity.Data = Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);
        entity.DataBin = dataObject?.ToByteArray();

        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    static bool? getProofOfIncomeToggle(_SA.UpdateIncomeRequest request, CustomerIncomeTypes typeId)
        => typeId switch
        {
            CustomerIncomeTypes.Employement => request.Employement?.HasProofOfIncome,
            _ => default
        };

    async Task<string?> getIncomeSource(_SA.UpdateIncomeRequest request, CustomerIncomeTypes typeId, CancellationToken cancellationToken)
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

    static IMessage getDataObject(CustomerIncomeTypes incomeType, _SA.UpdateIncomeRequest request)
        => incomeType switch
        {
            CustomerIncomeTypes.Employement => request.Employement,
            CustomerIncomeTypes.Other => request.Other,
            CustomerIncomeTypes.Enterprise => request.Entrepreneur,
            CustomerIncomeTypes.Rent => request.Rent,
            _ => throw new NotImplementedException("This customer income type serializer is not implemented")
        };

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Repositories.HouseholdServiceDbContext _dbContext;

    public UpdateIncomeHandler(Repositories.HouseholdServiceDbContext dbContext, CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}
