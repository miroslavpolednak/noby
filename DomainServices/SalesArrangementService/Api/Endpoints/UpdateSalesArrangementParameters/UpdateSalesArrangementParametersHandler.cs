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
            .Select(t => new { t.State, t.OfferGuaranteeDateTo })
            .FirstOrDefaultAsync(cancellation))
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // kontrolovat pokud je zmocnenec, tak zda existuje?
        if (request.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage)
        {
            if (request.Mortgage.Agent.HasValue)
            {
                var customersOnSA = await _customerOnSAService.GetCustomerList(request.SalesArrangementId, cancellation);
                if (!customersOnSA.Any(t => t.CustomerOnSAId == request.Mortgage.Agent))
                    throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.AgentNotFound, request.Mortgage.Agent);
            }
        }

        // instance parametru, pokud existuje
        var entity = await _dbContext
            .SalesArrangementsParameters
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
        else if (entity.SalesArrangementParametersType == SalesArrangementTypes.Drawing)
        {
            //Pokud se SA nezakládá (parameters v DB = null), tak validuj účet pro čerpání
            ValidateDrawingRepaymentAccount(request.Drawing, entity);
        }

        // naplnit parametry serializovanym objektem
        var dataObject = getDataObject(request);
        entity.Parameters = dataObject is null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(dataObject);
        entity.ParametersBin = dataObject is null ? null : dataObject.ToByteArray();

        await _dbContext.SaveChangesAsync(cancellation);

        // pokud je zadost NEW, zmenit na InProgress
        if (!request.DoNotUpdateSalesArrangementState && saInfoInstance.State == (int)SalesArrangementStates.NewArrangement)
        {
            await updateSalesArrangementState(request.SalesArrangementId, cancellation);
        }

        // set flow switches
        await setFlowSwitches(request.SalesArrangementId, saInfoInstance.OfferGuaranteeDateTo, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    async Task updateSalesArrangementState(int salesArrangementId, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.SalesArrangements
            .FirstAsync(t => t.SalesArrangementId == salesArrangementId, cancellationToken);

        entity.State = (int)SalesArrangementStates.InProgress;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    static SalesArrangementTypes getParameterType(Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase datacase)
        => datacase switch
        {
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage => SalesArrangementTypes.Mortgage,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Drawing => SalesArrangementTypes.Drawing,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.GeneralChange => SalesArrangementTypes.GeneralChange,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.HUBN => SalesArrangementTypes.HUBN,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.CustomerChange => SalesArrangementTypes.CustomerChange,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.CustomerChange3602A => SalesArrangementTypes.CustomerChange3602A,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.CustomerChange3602B => SalesArrangementTypes.CustomerChange3602B,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.CustomerChange3602C => SalesArrangementTypes.CustomerChange3602C,
            _ => throw new NotImplementedException($"UpdateSalesArrangementParametersRequest.DataOneofCase {datacase} is not implemented")
        };

    static IMessage? getDataObject(Contracts.UpdateSalesArrangementParametersRequest request)
        => request.DataCase switch
        {
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage => request.Mortgage,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Drawing => request.Drawing,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.GeneralChange => request.GeneralChange,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.HUBN => request.HUBN,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.CustomerChange => request.CustomerChange,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.CustomerChange3602A => request.CustomerChange3602A,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.CustomerChange3602B => request.CustomerChange3602B,
            Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.CustomerChange3602C => request.CustomerChange3602C,
            _ => null
        };

    /// <summary>
    /// Nastaveni flow switches v podle toho jak je nastavena simulace / sa
    /// </summary>
    private async Task setFlowSwitches(int salesArrangementId, DateTime? offerGuaranteeDateTo, CancellationToken cancellation)
    {
        if ((offerGuaranteeDateTo ?? DateTime.MinValue) > DateTime.Now)
        {
            var flowSwitchesRequest = new Contracts.SetFlowSwitchesRequest
            {
                SalesArrangementId = salesArrangementId
            };
            flowSwitchesRequest.FlowSwitches.Add(new Contracts.FlowSwitch
            {
                FlowSwitchId = (int)FlowSwitches.IsOfferGuaranteed,
                Value = true
            });
            await _mediator.Send(flowSwitchesRequest, cancellation);
        }
    }

    private static void ValidateDrawingRepaymentAccount(Contracts.SalesArrangementParametersDrawing drawingRequest, Database.Entities.SalesArrangementParameters originalParameters)
    {
        var originalAccount = Contracts.SalesArrangementParametersDrawing.Parser.ParseFrom(originalParameters.ParametersBin).RepaymentAccount;
        var requestAccount = drawingRequest.RepaymentAccount;

        if (originalAccount.IsAccountNumberMissing)
        {
            requestAccount.IsAccountNumberMissing = true;

            return;
        }

        var isAccountEqual = string.Equals(originalAccount.Prefix, requestAccount.Prefix, StringComparison.OrdinalIgnoreCase) &&
                             string.Equals(originalAccount.Number, requestAccount.Number, StringComparison.OrdinalIgnoreCase) &&
                             string.Equals(originalAccount.BankCode, requestAccount.BankCode, StringComparison.OrdinalIgnoreCase);

        if (!isAccountEqual)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.RepaymentAccountCantChange);

        requestAccount.IsAccountNumberMissing = false;
    }

    private readonly HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly IMediator _mediator;

    public UpdateSalesArrangementParametersHandler(
        IMediator mediator,
        HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        Database.SalesArrangementServiceDbContext dbContext)
    {
        _mediator = mediator;
        _customerOnSAService = customerOnSAService;
        _dbContext = dbContext;
    }
}
