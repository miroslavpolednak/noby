﻿using DomainServices.ProductService.Contracts;
using System.Threading;
using cCodebookService = DomainServices.CodebookService.Contracts;
using dto = FOMS.Api.Endpoints.Cases.GetCaseParameters.Dto;

namespace FOMS.Api.Endpoints.Cases.GetCaseParameters;

internal sealed class GetGetCaseParametersHandler
    : IRequestHandler<GetCaseParametersRequest, GetCaseParametersResponse>
{

    #region Construction

    private readonly DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly DomainServices.ProductService.Abstraction.IProductServiceAbstraction _productService;
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.UserService.Clients.IUserServiceAbstraction _userService;

    public GetGetCaseParametersHandler(
        DomainServices.CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        DomainServices.ProductService.Abstraction.IProductServiceAbstraction productService,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService,
        DomainServices.UserService.Clients.IUserServiceAbstraction userService
        )
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _productService = productService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _userService = userService;
    }

    #endregion

    public async Task<GetCaseParametersResponse> Handle(GetCaseParametersRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.CaseService.Contracts.Case>(await _caseService.GetCaseDetail(request.CaseId, cancellationToken));

        var caseStates = await _codebookService.CaseStates(cancellationToken);
        var productTypesById = (await _codebookService.ProductTypes(cancellationToken)).ToDictionary(i => i.Id);
        var loanKindsById = (await _codebookService.LoanKinds(cancellationToken)).ToDictionary(i => i.Id);

        var mandantId = productTypesById[caseInstance.Data.ProductTypeId].MandantId;
        var loanPurposesById = (await _codebookService.LoanPurposes(cancellationToken)).Where(i => i.MandantId == mandantId).ToDictionary(i => i.Id);

        var caseState = caseStates.First(i => i.Id == caseInstance.State);

        var response = (caseState.Code == CIS.Foms.Enums.CaseStates.InProgress) ?
            (await GetParamsBeforeHandover(caseInstance, productTypesById, loanKindsById, loanPurposesById, cancellationToken)) :
            (await GetParamsAfterHandover(caseInstance, productTypesById, loanKindsById, loanPurposesById, cancellationToken));

        return response;
    }

    private async Task<GetCaseParametersResponse> GetParamsBeforeHandover(
        DomainServices.CaseService.Contracts.Case caseInstance,
        Dictionary<int, cCodebookService.Endpoints.ProductTypes.ProductTypeItem> productTypesById,
        Dictionary<int, cCodebookService.Endpoints.LoanKinds.LoanKindsItem> loanKindsById,
        Dictionary<int, cCodebookService.Endpoints.LoanPurposes.LoanPurposesItem> loanPurposesById,
        CancellationToken cancellation
        )
    {
        /*
            How to get IDs of individual entities:
            - na vstupu je CaseId
            - použít službu SalesArrangement / GetSalesArrangementList
            - z listu vyfiltrovat SalesArrangement který má SalesArrangementTypeId z číselníku SalesArrangementType s atributem ProductTypeId <> null - bude jen jeden
            OfferId = SalesArrangement.OfferId
            ProductId = CaseId
            UserId = SalesArrangement.Created.UserId (mělo by být shodné s Case.CaseOwner.UserId)
            
            How to map data (entities -> parameters):
            // https://wiki.kb.cz/display/HT/Case+detail+-+Parametry+D1.3
        */

        // load SalesArrangement
        var saTypeIds = (await _codebookService.SalesArrangementTypes(cancellation)).Where(i => i.ProductTypeId.HasValue).Select(i => i.Id).ToList();
        var salesArrangementList = ServiceCallResult.ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementListResponse>(await _salesArrangementService.GetSalesArrangementList(caseInstance.CaseId, null, cancellation));
        var salesArrangementId = salesArrangementList.SalesArrangements.First(i => saTypeIds.Contains(i.SalesArrangementTypeId)).SalesArrangementId;
        var salesArrangementInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellation));

        // load Offer
        var offerId = salesArrangementInstance?.OfferId;
        var offerInstance = offerId.HasValue ? ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageOfferDetailResponse>(await _offerService.GetMortgageOfferDetail(offerId.Value, cancellation)) : null;

        // load User
        var userId = caseInstance.CaseOwner?.UserId;
        var userInstance = userId.HasValue ? ServiceCallResult.ResolveAndThrowIfError<DomainServices.UserService.Contracts.User>(await _userService.GetUser(userId.Value, cancellation)) : null;

        return new GetCaseParametersResponse
        {
            ProductType = productTypesById[offerInstance!.SimulationInputs.ProductTypeId].ToCodebookItem(),
            ContractNumber = salesArrangementInstance!.ContractNumber,
            LoanAmount = offerInstance!.SimulationResults.LoanAmount,
            LoanInterestRate = offerInstance!.SimulationResults.LoanInterestRateProvided,
            // ContractSignedDate
            // FixedRateValidTo
            // Principal
            // AvailableForDrawing
            DrawingDateTo = offerInstance!.SimulationResults.DrawingDateTo,
            LoanPaymentAmount = offerInstance!.SimulationResults.LoanPaymentAmount,
            LoanKind = loanKindsById[offerInstance!.SimulationInputs.LoanKindId].ToCodebookItem(),
            // CurrentAmount
            FixedRatePeriod = offerInstance!.SimulationInputs.FixedRatePeriod,
            // PaymentAccount
            // CurrentOverdueAmount
            // AllOverdueFees
            // OverdueDaysNumber
            LoanPurposes = offerInstance!.SimulationInputs.LoanPurposes.Select(i => new dto.LoanPurposeItem
            {
                LoanPurpose = loanPurposesById[i.LoanPurposeId].ToCodebookItem()!,
                Sum = i.Sum
            }).ToList(),
            ExpectedDateOfDrawing = salesArrangementInstance!.Mortgage?.ExpectedDateOfDrawing,
            // InterestInArrears
            LoanDueDate = offerInstance!.SimulationResults.LoanDueDate,
            PaymentDay = offerInstance!.SimulationInputs.PaymentDay,
            // LoanInterestRateRefix
            // LoanInterestRateValidFromRefix
            // FixedRatePeriodRefix
            Cpm = userInstance?.CPM,
            Icp = userInstance?.ICP,
        };
    }

    private async Task<GetCaseParametersResponse> GetParamsAfterHandover(
        DomainServices.CaseService.Contracts.Case caseInstance,
        Dictionary<int, cCodebookService.Endpoints.ProductTypes.ProductTypeItem> productTypesById,
        Dictionary<int, cCodebookService.Endpoints.LoanKinds.LoanKindsItem> loanKindsById,
        Dictionary<int, cCodebookService.Endpoints.LoanPurposes.LoanPurposesItem> loanPurposesById,
        CancellationToken cancellation
        )
    {
        var mortgageData = ServiceCallResult.ResolveAndThrowIfError<GetMortgageResponse>(await _productService.GetMortgage(caseInstance.CaseId, cancellation)).Mortgage;

        return new GetCaseParametersResponse
        {
            ProductType = productTypesById[mortgageData.ProductTypeId].ToCodebookItem(),
            ContractNumber = mortgageData.ContractNumber,
            LoanAmount = mortgageData.LoanAmount,
            LoanInterestRate = mortgageData.LoanInterestRate,
            ContractSignedDate = mortgageData.ContractSignedDate,
            FixedRateValidTo = mortgageData.FixedRateValidTo,
            Principal = mortgageData.Principal,
            AvailableForDrawing = mortgageData.AvailableForDrawing,
            DrawingDateTo = mortgageData.DrawingDateTo,
            LoanPaymentAmount = mortgageData.LoanPaymentAmount,
            LoanKind = mortgageData.LoanKindId.HasValue ? loanKindsById[mortgageData.LoanKindId.Value].ToCodebookItem() : null,
            CurrentAmount = mortgageData.CurrentAmount,
            FixedRatePeriod = mortgageData.FixedRatePeriod,
            PaymentAccount = mortgageData.PaymentAccount.ToPaymentAccount(),
            CurrentOverdueAmount = mortgageData.CurrentOverdueAmount,
            AllOverdueFees = mortgageData.AllOverdueFees,
            OverdueDaysNumber = mortgageData.OverdueDaysNumber,
            LoanPurposes = mortgageData.LoanPurposes.Select(i=> new dto.LoanPurposeItem
            {
                LoanPurpose = loanPurposesById[i.LoanPurposeId].ToCodebookItem()!,
                Sum = i.Sum
            }).ToList(),
            ExpectedDateOfDrawing = mortgageData.ExpectedDateOfDrawing,
            InterestInArrears = mortgageData.InterestInArrears,
            LoanDueDate = mortgageData.LoanDueDate,
            PaymentDay = mortgageData.PaymentDay,
            LoanInterestRateRefix = mortgageData.LoanInterestRateRefix,
            LoanInterestRateValidFromRefix = mortgageData.LoanInterestRateValidFromRefix,
            FixedRatePeriodRefix = mortgageData.FixedRatePeriodRefix,
            Cpm = mortgageData.Cpm,
            Icp = mortgageData.Icp,
        };
    }
}
