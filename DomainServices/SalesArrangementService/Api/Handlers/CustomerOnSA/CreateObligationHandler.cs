using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;
using Google.Protobuf;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class CreateObligationHandler
    : IRequestHandler<Dto.CreateObligationMediatrRequest, CreateObligationResponse>
{
    public async Task<CreateObligationResponse> Handle(Dto.CreateObligationMediatrRequest request, CancellationToken cancellation)
    {
        // check customer existence
        if (!await _dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == request.Request.CustomerOnSAId, cancellation))
            throw new CisNotFoundException(16020, "CustomerOnSA", request.Request.CustomerOnSAId);

        var entity = new Repositories.Entities.CustomerOnSAIncome
        {
            CustomerOnSAId = request.Request.CustomerOnSAId,
            Sum = request.Request.BaseData?.Sum,
            CurrencyCode = request.Request.BaseData?.CurrencyCode,
            IncomeSource = getIncomeSource(request.Request),
            IncomeTypeId = (CIS.Foms.Enums.CustomerIncomeTypes)request.Request.IncomeTypeId
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

    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    private readonly ILogger<CreateObligationHandler> _logger;

    public CreateObligationHandler(
        Repositories.SalesArrangementServiceDbContext dbContext,
        ILogger<CreateObligationHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}
