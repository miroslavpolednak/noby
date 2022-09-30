using Microsoft.EntityFrameworkCore;
using Google.Protobuf;
using _HO = DomainServices.HouseholdService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement;

internal class UpdateSalesArrangementParametersHandler
    : IRequestHandler<Dto.UpdateSalesArrangementParametersMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementParametersMediatrRequest request, CancellationToken cancellation)
    {
        // existuje SA?
        if (!await _dbContext.SalesArrangements.AnyAsync(t => t.SalesArrangementId == request.Request.SalesArrangementId, cancellation))
            throw new CisNotFoundException(16000, $"Sales arrangement ID {request.Request.SalesArrangementId} does not exist.");

        // kontrolovat pokud je zmocnenec, tak zda existuje?
        if (request.Request.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage)
        {
            if (request.Request.Mortgage.Agent.HasValue)
            {
                var customersOnSA = ServiceCallResult.ResolveAndThrowIfError<_HO.GetCustomerListResponse>(await _customerOnSAService.GetCustomerList(request.Request.SalesArrangementId, cancellation));
                customersOnSA.Customers.Any(t => t.CustomerOnSAId)
                if (!_dbContext.Customers.Any(t => t.SalesArrangementId == request.Request.SalesArrangementId && t.CustomerOnSAId == request.Request.Mortgage.Agent))
                    throw new CisNotFoundException(16078, $"Agent {request.Request.Mortgage.Agent} not found amoung customersOnSA for SAID {request.Request.SalesArrangementId}");
            }
        }
        
        // instance parametru, pokud existuje
        var entity = await _dbContext.SalesArrangementsParameters.FirstOrDefaultAsync(t => t.SalesArrangementId == request.Request.SalesArrangementId, cancellation);
        if (entity is null)
        {
            entity = new Repositories.Entities.SalesArrangementParameters
            {
                SalesArrangementId = request.Request.SalesArrangementId
            };
            _dbContext.SalesArrangementsParameters.Add(entity);
        }

        // naplnit parametry serializovanym objektem
        var dataObject = getDataObject(request.Request);
        entity.Parameters = dataObject is null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);
        entity.ParametersBin = dataObject is null ? null : dataObject.ToByteArray();
        
        await _dbContext.SaveChangesAsync(cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    static IMessage? getDataObject(Contracts.UpdateSalesArrangementParametersRequest request)
        => request.DataCase switch
        {
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage => request.Mortgage,
            _ => null
        };

    private readonly HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly Repositories.SalesArrangementServiceDbContext _dbContext;
    
    public UpdateSalesArrangementParametersHandler(
        HouseholdService.Clients.IHouseholdServiceClient householdService,
        Repositories.SalesArrangementServiceDbContext dbContext)
    {
        _householdService = householdService;
        _dbContext = dbContext;
    }
}
