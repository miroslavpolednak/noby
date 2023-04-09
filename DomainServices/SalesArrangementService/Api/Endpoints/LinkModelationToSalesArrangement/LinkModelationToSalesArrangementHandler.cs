using CIS.Foms.Enums;
using Google.Protobuf;
using Microsoft.EntityFrameworkCore;
using __Offer = DomainServices.OfferService.Contracts;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Endpoints.LinkModelationToSalesArrangement;

internal sealed class LinkModelationToSalesArrangementHandler
    : IRequestHandler<__SA.LinkModelationToSalesArrangementRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(__SA.LinkModelationToSalesArrangementRequest request, CancellationToken cancellation)
    {
        // overit existenci SA
        var salesArrangementInstance = await _dbContext
            .SalesArrangements
            .FirstOrDefaultAsync(t => t.SalesArrangementId == request.SalesArrangementId, cancellation)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.SalesArrangementNotFound, request.SalesArrangementId);

        // kontrola zda SA uz neni nalinkovan na stejnou Offer na kterou je request
        if (salesArrangementInstance.OfferId == request.OfferId)
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.AlreadyLinkedToOffer, request.SalesArrangementId);

        // instance Case
        var caseInstance = await _caseService.GetCaseDetail(salesArrangementInstance.CaseId, cancellation);

        // validace na existenci offer
        var offerInstance = await _offerService.GetMortgageOffer(request.OfferId, cancellation);

        // kontrola, zda simulace neni nalinkovana na jiny SA
        if (await _dbContext.SalesArrangements.AnyAsync(t => t.OfferId == request.OfferId, cancellation))
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.AlreadyLinkedToAnotherSA, request.OfferId);

        // Kontrola, že nová Offer má GuaranteeDateFrom větší nebo stejné jako původně nalinkovaná offer
        if (salesArrangementInstance.OfferId.HasValue)
        {
            var offerInstanceOld = await _offerService.GetMortgageOffer(salesArrangementInstance.OfferId.Value, cancellation);
            if ((DateTime)offerInstance.SimulationInputs.GuaranteeDateFrom < (DateTime)offerInstanceOld.SimulationInputs.GuaranteeDateFrom)
                throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.InvalidGuaranteeDateFrom);
        }

        // update linku v DB
        salesArrangementInstance.OfferGuaranteeDateFrom = offerInstance.SimulationInputs.GuaranteeDateFrom;
        salesArrangementInstance.OfferGuaranteeDateTo = offerInstance.BasicParameters.GuaranteeDateTo;
        salesArrangementInstance.OfferId = request.OfferId;
        salesArrangementInstance.ResourceProcessId = Guid.Parse(offerInstance.ResourceProcessId);
        // HFICH-3391
        salesArrangementInstance.OfferDocumentId = null;

        await _dbContext.SaveChangesAsync(cancellation);

        // update parametru
        await updateParameters(salesArrangementInstance, offerInstance, cancellation);

        // HFICH-3088 - Dashboard - Výši úvěru na case je třeba updatovat při přelinkování simulace
        await _caseService.UpdateCaseData(salesArrangementInstance.CaseId, new CaseService.Contracts.CaseData
        {
            ContractNumber = caseInstance.Data.ContractNumber,
            ProductTypeId = caseInstance.Data.ProductTypeId,
            TargetAmount = offerInstance.SimulationInputs.LoanAmount,
            IsEmployeeBonusRequested = offerInstance.SimulationInputs.IsEmployeeBonusRequested
        }, cancellation);

        // nastavit flowSwitches
        await setFlowSwitches(request.SalesArrangementId, offerInstance, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    /// <summary>
    /// Nastaveni flow switches v podle toho jak je nastavena simulace / sa
    /// </summary>
    private async Task setFlowSwitches(int salesArrangementId, __Offer.GetMortgageOfferResponse offerInstance, CancellationToken cancellation)
    {
        if (((DateTime?)offerInstance.BasicParameters.GuaranteeDateTo ?? DateTime.MinValue) > DateTime.Now)
        {
            var flowSwitchesRequest = new Contracts.SetFlowSwitchesRequest
            {
                SalesArrangementId = salesArrangementId
            };
            flowSwitchesRequest.FlowSwitches.Add(new __SA.FlowSwitch
            {
                FlowSwitchId = (int)FlowSwitches.FlowSwitch1,
                Value = true
            });
            await _mediator.Send(flowSwitchesRequest, cancellation);
        }
    }

    private async Task updateParameters(Database.Entities.SalesArrangement salesArrangementInstance, __Offer.GetMortgageOfferResponse offerInstance, CancellationToken cancellation)
    {
        // parametry SA
        bool hasChanged = false;
        var saParameters = await _dbContext.SalesArrangementsParameters.FirstOrDefaultAsync(t => t.SalesArrangementId == salesArrangementInstance.SalesArrangementId, cancellation);
        if (saParameters is null)
        {
            saParameters = new Database.Entities.SalesArrangementParameters
            {
                SalesArrangementId = salesArrangementInstance.SalesArrangementId,
                SalesArrangementParametersType = SalesArrangementTypes.Mortgage
            };
            _dbContext.SalesArrangementsParameters.Add(saParameters);
        }
        var parametersModel = saParameters?.ParametersBin is not null ? __SA.SalesArrangementParametersMortgage.Parser.ParseFrom(saParameters.ParametersBin) : new __SA.SalesArrangementParametersMortgage();

        if (offerInstance.SimulationInputs.LoanKindId == 2001 ||
            offerInstance.SimulationInputs.LoanPurposes is not null && offerInstance.SimulationInputs.LoanPurposes.Any(t => t.LoanPurposeId == 201))
        {
            parametersModel.LoanRealEstates.Clear();
            hasChanged = true;
        }

        // HFICH-2181
        if (parametersModel.ExpectedDateOfDrawing != null || (parametersModel.ExpectedDateOfDrawing == null && offerInstance.SimulationInputs.ExpectedDateOfDrawing > DateTime.Now.AddDays(1)))
        {
            parametersModel.ExpectedDateOfDrawing = offerInstance.SimulationInputs.ExpectedDateOfDrawing;
            hasChanged = true;
        }

        if (hasChanged)
        {
            saParameters!.Parameters = Newtonsoft.Json.JsonConvert.SerializeObject(parametersModel);
            saParameters.ParametersBin = parametersModel.ToByteArray();
            await _dbContext.SaveChangesAsync(cancellation);
        }
    }

    private readonly CaseService.Clients.ICaseServiceClient _caseService;
    private readonly OfferService.Clients.IOfferServiceClient _offerService;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly IMediator _mediator;

    public LinkModelationToSalesArrangementHandler(
        IMediator mediator,
        CaseService.Clients.ICaseServiceClient caseService,
        Database.SalesArrangementServiceDbContext dbContext,
        OfferService.Clients.IOfferServiceClient offerService)
    {
        _mediator = mediator;
        _caseService = caseService;
        _dbContext = dbContext;
        _offerService = offerService;
    }
}
