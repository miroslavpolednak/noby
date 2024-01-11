﻿using DomainServices.RealEstateValuationService.Clients;
using Microsoft.EntityFrameworkCore;
using __Offer = DomainServices.OfferService.Contracts;
using __SA = DomainServices.SalesArrangementService.Contracts;
using DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;
using SharedComponents.DocumentDataStorage;

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
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.AlreadyLinkedToOffer, request.SalesArrangementId);
        }

        // instance Case
        var caseInstance = await _caseService.GetCaseDetail(salesArrangementInstance.CaseId, cancellation);

        // validace na existenci offer
        var offerInstance = await _offerService.GetMortgageOffer(request.OfferId, cancellation);

        // instance puvodni Offer
        __Offer.GetMortgageOfferResponse? offerInstanceOld = null;
        if (salesArrangementInstance.OfferId.HasValue)
        {
            offerInstanceOld = await _offerService.GetMortgageOffer(salesArrangementInstance.OfferId.Value, cancellation);
        }

        // kontrola, zda simulace neni nalinkovana na jiny SA
        if (await _dbContext.SalesArrangements.AnyAsync(t => t.OfferId == request.OfferId, cancellation))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.AlreadyLinkedToAnotherSA, request.OfferId);
        }

        // Kontrola, že nová Offer má GuaranteeDateFrom větší nebo stejné jako původně nalinkovaná offer
        if (offerInstanceOld is not null
            && (DateTime)offerInstance.SimulationInputs.GuaranteeDateFrom < (DateTime)offerInstanceOld.SimulationInputs.GuaranteeDateFrom)
        {
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
        await setFlowSwitches(salesArrangementInstance.CaseId, request.SalesArrangementId, offerInstance, offerInstanceOld, cancellation);

        // Aktualizace dat modelace v KonsDB pouze pro premodelaci
        if (offerInstanceOld is not null && caseInstance.Customer.Identity is not null && caseInstance.Customer.Identity.IdentityId > 0)
        {
            await _productService.UpdateMortgage(salesArrangementInstance.CaseId, cancellation);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    /// <summary>
    /// Nastaveni flow switches v podle toho jak je nastavena simulace / sa
    /// </summary>
    private async Task setFlowSwitches(long caseId, int salesArrangementId, __Offer.GetMortgageOfferResponse offerInstance, __Offer.GetMortgageOfferResponse? offerInstanceOld, CancellationToken cancellation)
    {
        List<__SA.EditableFlowSwitch> flowSwitchesToSet = new();

        // Pokud existuje sleva na poplatku nebo sazbě (libovolný poplatek DiscountPercentage z kolekce SimulationInputs.Fees > 0 nebo SimulationInputs.InterestRateDiscount > 0),
        bool isOfferWithDiscount = (offerInstance.SimulationInputs.Fees?.Any(t => t.DiscountPercentage > 0) ?? false) || (offerInstance.SimulationInputs.InterestRateDiscount ?? 0) > 0M;
        flowSwitchesToSet.Add(new()
        {
            FlowSwitchId = (int)FlowSwitches.IsOfferWithDiscount,
            Value = isOfferWithDiscount
        });

        // Pokud Offer.BasicParameters.GuranteeDateTo > sysdate (tedy platnost garance ještě neskončila)
        if (((DateTime?)offerInstance.BasicParameters.GuaranteeDateTo ?? DateTime.MinValue) > DateTime.Now)
        {
            flowSwitchesToSet.Add(new()
            {
                FlowSwitchId = (int)FlowSwitches.IsOfferGuaranteed,
                Value = true
            });
        }

        // HFICH-9611
        if (offerInstanceOld is not null
            && (
                (offerInstanceOld.SimulationInputs.LoanKindId == 2001 && offerInstance.SimulationInputs.LoanKindId == 2000)
                || (offerInstanceOld.SimulationInputs.LoanPurposes.All(t => t.LoanPurposeId == 201) && offerInstance.SimulationInputs.LoanPurposes.Any(t => t.LoanPurposeId != 201)))
            )
        {
            flowSwitchesToSet.Add(new()
            {
                FlowSwitchId = (int)FlowSwitches.ParametersSavedAtLeastOnce,
                Value = false
            });
        }

        //  Pokud parametry původně nalinkované Offer (konkrétně parametr Offer.BasicParameters.GuranteeDateTo, všechny DiscountPercentage z kolekce SimulationInputs.Fees a parametr SimulationInputs.InterestRateDiscount), nejsou stejné jako  odpovídající parametry na nové offer
        if (offerInstanceOld is not null)
        {
            bool isSwitch8On = await _dbContext
                .FlowSwitches
                .AnyAsync(t => t.SalesArrangementId == salesArrangementId && t.FlowSwitchId == (int)FlowSwitches.DoesWflTaskForIPExist && t.Value, cancellation);
            var fee1 = offerInstance.SimulationInputs.Fees?.Select(t => (decimal)t.DiscountPercentage).ToArray() ?? Array.Empty<decimal>();
            var fee2 = offerInstanceOld.SimulationInputs.Fees?.Select(t => (decimal)t.DiscountPercentage).ToArray() ?? Array.Empty<decimal>();

            if (isSwitch8On
                && (!Equals(offerInstance.BasicParameters.GuaranteeDateTo, offerInstanceOld.BasicParameters.GuaranteeDateTo)
                || !Equals(offerInstance.SimulationInputs.InterestRateDiscount, offerInstanceOld.SimulationInputs.InterestRateDiscount)
                || fee1.Except(fee2).Union(fee2.Except(fee1)).Any()))
            {
                // Pokud již existuje WFL úkol na IC (getTaskList zafiltrovat na TaskTypeId = 2 a Cancelled = false), pak dojde k jeho zrušení pomocí cancelTask (na vstup jde TaskIdSB)
                var taskList = await _caseService.GetTaskList(caseId, cancellation);
                var taskToCancel = taskList.FirstOrDefault(t => t.TaskTypeId == 2 && !t.Cancelled);
                if (taskToCancel is not null)
                {
                    await _caseService.CancelTask(caseId, taskToCancel.TaskIdSb, cancellation);
                }
            }
        }

        // Nastavení klapky k ocenění nemovitosti
        bool flowSwitch15 = offerInstance.SimulationInputs.LoanKindId == 2000;
        if (!flowSwitch15)
        {
            // smazat oceneni nemovitosti
            await deleteBoundedRealEstateValuations(caseId, cancellation);
        }
        flowSwitchesToSet.Add(new __SA.EditableFlowSwitch
        {
            FlowSwitchId = (int)FlowSwitches.IsRealEstateValuationAllowed,
            Value = flowSwitch15
        });

        // ulozit pokud byli nejake switches nastaveny
        if (flowSwitchesToSet.Count != 0)
        {
            var flowSwitchesRequest = new Contracts.SetFlowSwitchesRequest
            {
                SalesArrangementId = salesArrangementId
            };
            flowSwitchesRequest.FlowSwitches.AddRange(flowSwitchesToSet);

            await _mediator.Send(flowSwitchesRequest, cancellation);
        }
    }

    private async Task deleteBoundedRealEstateValuations(long caseId, CancellationToken cancellationToken)
    {
        var realEstatesToDelete = (await _realEstateValuationService.GetRealEstateValuationList(caseId, cancellationToken))
                .Where(t => t.ValuationStateId is (int)RealEstateValuationStates.Neoceneno or (int)RealEstateValuationStates.Rozpracovano);
        foreach (var realEstateValuation in realEstatesToDelete)
        {
            try
            {
                await _realEstateValuationService.DeleteRealEstateValuation(caseId, realEstateValuation.RealEstateValuationId, cancellationToken);
            }
            catch { }
        }
    }

    private async Task updateParameters(Database.Entities.SalesArrangement salesArrangementInstance, __Offer.GetMortgageOfferResponse offerInstance, CancellationToken cancellation)
    {
        // parametry SA
        var hasChanged = false;

        var saParametersDocument = await _documentDataStorage.FirstOrDefaultByEntityId<MortgageData>(salesArrangementInstance.SalesArrangementTypeId, SalesArrangementParametersConst.TableName, cancellation);

        var parametersModel = saParametersDocument?.Data ?? new MortgageData();

        if (offerInstance.SimulationInputs.LoanKindId == 2001 ||
            offerInstance.SimulationInputs.LoanPurposes is not null && offerInstance.SimulationInputs.LoanPurposes.Any(t => t.LoanPurposeId == 201))
        {
            parametersModel.LoanRealEstates.Clear();
            hasChanged = true;
        }

        // HFICH-2181
        if (parametersModel.ExpectedDateOfDrawing != null || parametersModel.ExpectedDateOfDrawing == null && offerInstance.SimulationInputs.ExpectedDateOfDrawing > DateTime.Now.AddDays(1))
        {
            parametersModel.ExpectedDateOfDrawing = offerInstance.SimulationInputs.ExpectedDateOfDrawing;
            hasChanged = true;
        }

        if (hasChanged) 
            await _documentDataStorage.AddOrUpdateByEntityId(salesArrangementInstance.SalesArrangementId, SalesArrangementParametersConst.TableName, parametersModel, cancellation);
    }

    private readonly ProductService.Clients.IProductServiceClient _productService;
    private readonly CaseService.Clients.ICaseServiceClient _caseService;
    private readonly OfferService.Clients.IOfferServiceClient _offerService;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public LinkModelationToSalesArrangementHandler(
        ProductService.Clients.IProductServiceClient productService,
        IRealEstateValuationServiceClient realEstateValuationService,
        IMediator mediator,
        CaseService.Clients.ICaseServiceClient caseService,
        Database.SalesArrangementServiceDbContext dbContext,
        OfferService.Clients.IOfferServiceClient offerService,
        IDocumentDataStorage documentDataStorage)
    {
        _productService = productService;
        _realEstateValuationService = realEstateValuationService;
        _mediator = mediator;
        _caseService = caseService;
        _dbContext = dbContext;
        _offerService = offerService;
        _documentDataStorage = documentDataStorage;
    }
}
