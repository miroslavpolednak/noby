using DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementParameters;

internal sealed class UpdateSalesArrangementParametersHandler : IRequestHandler<Contracts.UpdateSalesArrangementParametersRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellationToken)
    {
        // existuje SA?
        var saInfoInstance = await _dbContext.SalesArrangements
                                             .Where(t => t.SalesArrangementId == request.SalesArrangementId)
                                             .Select(t => new { t.SalesArrangementTypeId, t.State, t.OfferGuaranteeDateTo })
                                             .FirstOrDefaultAsync(cancellationToken)
                             ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // kontrolovat pokud je zmocnenec, tak zda existuje?
        if (request.DataCase == Contracts.UpdateSalesArrangementParametersRequest.DataOneofCase.Mortgage)
        {
            if (request.Mortgage.Agent.HasValue)
            {
                var customersOnSA = await _customerOnSAService.GetCustomerList(request.SalesArrangementId, cancellationToken);

                if (customersOnSA.All(t => t.CustomerOnSAId != request.Mortgage.Agent))
                    throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.AgentNotFound, request.Mortgage.Agent);
            }
        }

        if (saInfoInstance.SalesArrangementTypeId == (int)SalesArrangementTypes.Drawing)
        {
            var drawingParameters = await _documentDataStorage.FirstOrDefaultByEntityId<DrawingData>(request.SalesArrangementId, SalesArrangementParametersConst.TableName, cancellationToken);

            if (drawingParameters is not null)
                ValidateDrawingRepaymentAccount(request.Drawing, drawingParameters.Data);
        }

        // SA parameters
        await AddOrUpdateParameters((SalesArrangementTypes)saInfoInstance.SalesArrangementTypeId, request, cancellationToken);

        // set flow switches
        await SetFlowSwitches(request.SalesArrangementId, saInfoInstance.OfferGuaranteeDateTo, cancellationToken);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    /// <summary>
    /// Nastaveni flow switches v podle toho jak je nastavena simulace / sa
    /// </summary>
    private async Task SetFlowSwitches(int salesArrangementId, DateTime? offerGuaranteeDateTo, CancellationToken cancellationToken)
    {
        if ((offerGuaranteeDateTo ?? DateTime.MinValue) <= DateTime.Now)
            return;

        var flowSwitchesRequest = new Contracts.SetFlowSwitchesRequest
        {
            SalesArrangementId = salesArrangementId,
            FlowSwitches =
            {
                new Contracts.EditableFlowSwitch
                {
                    FlowSwitchId = (int)FlowSwitches.IsOfferGuaranteed,
                    Value = true
                }
            }
        };

        await _mediator.Send(flowSwitchesRequest, cancellationToken);
    }

    private static void ValidateDrawingRepaymentAccount(Contracts.SalesArrangementParametersDrawing drawingRequest, DrawingData? originalParameters)
    {
        var originalAccount = originalParameters?.RepaymentAccount ?? new DrawingData.DrawingRepaymentAccount();
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

    private Task<int> AddOrUpdateParameters(SalesArrangementTypes salesArrangementType, Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellationToken)
    {
        return salesArrangementType switch
        {
            SalesArrangementTypes.Mortgage => _documentDataStorage.AddOrUpdateByEntityId(request.SalesArrangementId, SalesArrangementParametersConst.TableName, request.Mortgage.MapMortgage(), cancellationToken),
            SalesArrangementTypes.Drawing => _documentDataStorage.AddOrUpdateByEntityId(request.SalesArrangementId, SalesArrangementParametersConst.TableName, request.Drawing.MapDrawing(), cancellationToken),
            SalesArrangementTypes.GeneralChange => _documentDataStorage.AddOrUpdateByEntityId(request.SalesArrangementId, SalesArrangementParametersConst.TableName, request.GeneralChange.MapGeneralChange(), cancellationToken),
            SalesArrangementTypes.HUBN => _documentDataStorage.AddOrUpdateByEntityId(request.SalesArrangementId, SalesArrangementParametersConst.TableName, request.HUBN.MapHUBN(), cancellationToken),
            SalesArrangementTypes.CustomerChange => _documentDataStorage.AddOrUpdateByEntityId(request.SalesArrangementId, SalesArrangementParametersConst.TableName, request.CustomerChange.MapCustomerChange(), cancellationToken),
            SalesArrangementTypes.CustomerChange3602A => _documentDataStorage.AddOrUpdateByEntityId(request.SalesArrangementId, SalesArrangementParametersConst.TableName, request.CustomerChange3602A.MapCustomerChange3602(), cancellationToken),
            SalesArrangementTypes.CustomerChange3602B => _documentDataStorage.AddOrUpdateByEntityId(request.SalesArrangementId, SalesArrangementParametersConst.TableName, request.CustomerChange3602B.MapCustomerChange3602(), cancellationToken),
            SalesArrangementTypes.CustomerChange3602C => _documentDataStorage.AddOrUpdateByEntityId(request.SalesArrangementId, SalesArrangementParametersConst.TableName, request.CustomerChange3602C.MapCustomerChange3602(), cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(salesArrangementType), salesArrangementType, null)
        };
    }

    private readonly HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IMediator _mediator;

    public UpdateSalesArrangementParametersHandler(
        IMediator mediator,
        HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        Database.SalesArrangementServiceDbContext dbContext,
        IDocumentDataStorage documentDataStorage)
    {
        _mediator = mediator;
        _customerOnSAService = customerOnSAService;
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
    }
}
