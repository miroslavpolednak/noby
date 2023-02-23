using Microsoft.EntityFrameworkCore;
using Google.Protobuf;
using CIS.Foms.Enums;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementParameters;

internal sealed class UpdateSalesArrangementParametersHandler
    : IRequestHandler<Contracts.UpdateSalesArrangementParametersRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellation)
    {
        // existuje SA?
        var saInfoInstance = (await _dbContext.SalesArrangements
            .Where(t => t.SalesArrangementId == request.SalesArrangementId)
            .Select(t => new { t.State })
            .FirstOrDefaultAsync(cancellation))
            ?? throw new CisNotFoundException(18000, $"Sales arrangement ID {request.SalesArrangementId} does not exist.");

        // kontrolovat pokud je zmocnenec, tak zda existuje?
        if (request.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage)
        {
            if (request.Mortgage.Agent.HasValue)
            {
                var customersOnSA = await _customerOnSAService.GetCustomerList(request.SalesArrangementId, cancellation);
                if (!customersOnSA.Any(t => t.CustomerOnSAId == request.Mortgage.Agent))
                    throw new CisNotFoundException(18078, $"Agent {request.Mortgage.Agent} not found amoung customersOnSA for SAID {request.SalesArrangementId}");
            }
        }

        // instance parametru, pokud existuje
        var entity = await _dbContext.SalesArrangementsParameters
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation);

        if (entity is null)
        {
            entity = new Database.Entities.SalesArrangementParameters
            {
                SalesArrangementId = request.SalesArrangementId,
                SalesArrangementParametersType = getParameterType(request.DataCase)
            };
            _dbContext.SalesArrangementsParameters.Add(entity);
        }

        // naplnit parametry serializovanym objektem
        var dataObject = getDataObject(request);
        entity.Parameters = dataObject is null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);
        entity.ParametersBin = dataObject is null ? null : dataObject.ToByteArray();

        await _dbContext.SaveChangesAsync(cancellation);

        // pokud je zadost NEW, zmenit na InProgress
        if (saInfoInstance.State == (int)SalesArrangementStates.NewArrangement)
        {
            await updateSalesArrangementState(request.SalesArrangementId, cancellation);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    async Task updateSalesArrangementState(int salesArrangementId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.SalesArrangements
            .FirstAsync(t => t.SalesArrangementId == salesArrangementId, cancellationToken);

        entity.State = (int)SalesArrangementStates.InProgress;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    static Database.Entities.SalesArrangementParametersTypes getParameterType(Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase datacase)
        => datacase switch
        {
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage => Database.Entities.SalesArrangementParametersTypes.Mortgage,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Drawing => Database.Entities.SalesArrangementParametersTypes.Drawing,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.GeneralChange => Database.Entities.SalesArrangementParametersTypes.GeneralChange,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.HUBN => Database.Entities.SalesArrangementParametersTypes.HUBN,
            _ => throw new NotImplementedException($"UpdateSalesArrangementParametersRequest.DataOneofCase {datacase} is not implemented")
        };

    static IMessage? getDataObject(Contracts.UpdateSalesArrangementParametersRequest request)
        => request.DataCase switch
        {
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage => request.Mortgage,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Drawing => request.Drawing,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.GeneralChange => request.GeneralChange,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.HUBN => request.HUBN,
            _ => null
        };

    private readonly HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public UpdateSalesArrangementParametersHandler(
        HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        Database.SalesArrangementServiceDbContext dbContext)
    {
        _customerOnSAService = customerOnSAService;
        _dbContext = dbContext;
    }
}
