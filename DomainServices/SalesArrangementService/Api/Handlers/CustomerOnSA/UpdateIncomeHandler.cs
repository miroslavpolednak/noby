using _SA = DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using Google.Protobuf;
using CIS.Foms.Enums;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal sealed class UpdateIncomeHandler
    : IRequestHandler<Dto.UpdateIncomeMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateIncomeMediatrRequest request, CancellationToken cancellation)
    {
        var entity = (await _dbContext.CustomersIncomes
            .Where(t => t.CustomerOnSAIncomeId == request.Request.IncomeId)
            .FirstOrDefaultAsync(cancellation)) ?? throw new CisNotFoundException(16029, $"Income ID {request.Request.IncomeId} does not exist.");

        var dataObject = getDataObject(entity.IncomeTypeId, request.Request);
        entity.Sum = request.Request.BaseData?.Sum;
        entity.CurrencyCode = request.Request.BaseData?.CurrencyCode;
        entity.IncomeSource = await getIncomeSource(request.Request, entity.IncomeTypeId, cancellation);
        entity.ProofOfIncomeToggle = getProofOfIncomeToggle(request.Request, entity.IncomeTypeId);
        entity.Data = Newtonsoft.Json.JsonConvert.SerializeObject((object)dataObject);
        entity.DataBin = dataObject?.ToByteArray();

        await _dbContext.SaveChangesAsync(cancellation);
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    static bool? getProofOfIncomeToggle(_SA.UpdateIncomeRequest request, CustomerIncomeTypes typeId)
        => typeId switch
        {
            CustomerIncomeTypes.Employement =>request.Employement?.ProofOfIncomeToggle,
            _ => default(bool?)
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

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;

    public UpdateIncomeHandler(Repositories.SalesArrangementServiceDbContext dbContext, CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _dbContext = dbContext;
        _codebookService = codebookService;
    }
}
