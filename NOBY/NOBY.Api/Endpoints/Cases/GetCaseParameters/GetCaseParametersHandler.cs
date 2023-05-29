﻿using NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;
using cCodebookService = DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

internal sealed class GetCaseParametersHandler
    : IRequestHandler<GetCaseParametersRequest, GetCaseParametersResponse>
{

    #region Construction

    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;
    private readonly DomainServices.ProductService.Clients.IProductServiceClient _productService;
    private readonly DomainServices.OfferService.Clients.IOfferServiceClient _offerService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;

    public GetCaseParametersHandler(
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService,
        DomainServices.ProductService.Clients.IProductServiceClient productService,
        DomainServices.OfferService.Clients.IOfferServiceClient offerService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        DomainServices.UserService.Clients.IUserServiceClient userService
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
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        var caseStates = await _codebookService.CaseStates(cancellationToken);
        var productTypesById = (await _codebookService.ProductTypes(cancellationToken)).ToDictionary(i => i.Id);
        var loanKindsById = (await _codebookService.LoanKinds(cancellationToken)).ToDictionary(i => i.Id);
        var statementFrequencies = (await _codebookService.StatementFrequencies(cancellationToken));
        var statementSubscriptionTypes = (await _codebookService.StatementSubscriptionTypes(cancellationToken));
        var statementTypes = (await _codebookService.StatementTypes(cancellationToken));

        var mandantId = productTypesById[caseInstance.Data.ProductTypeId].MandantId;
        var loanPurposesById = (await _codebookService.LoanPurposes(cancellationToken)).Where(i => i.MandantId == mandantId).ToDictionary(i => i.Id);

        var caseState = caseStates.First(i => i.Id == caseInstance.State);

        var response = (caseState.Id == (int)CIS.Foms.Enums.CaseStates.InProgress) ?
            (await GetParamsBeforeHandover(caseInstance, productTypesById, loanKindsById, loanPurposesById, cancellationToken)) :
            (await GetParamsAfterHandover(caseInstance, productTypesById, loanKindsById, loanPurposesById, statementTypes, statementSubscriptionTypes, statementFrequencies, cancellationToken));

        return response;
    }

    private async Task<GetCaseParametersResponse> GetParamsBeforeHandover(
        DomainServices.CaseService.Contracts.Case caseInstance,
        Dictionary<int, cCodebookService.ProductTypesResponse.Types.ProductTypeItem> productTypesById,
        Dictionary<int, cCodebookService.GenericCodebookResponse.Types.GenericCodebookItem> loanKindsById,
        Dictionary<int, cCodebookService.LoanPurposesResponse.Types.LoanPurposeItem> loanPurposesById,
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
        var salesArrangementList = await _salesArrangementService.GetSalesArrangementList(caseInstance.CaseId, cancellation);
        var salesArrangementId = salesArrangementList.SalesArrangements.First(i => saTypeIds.Contains(i.SalesArrangementTypeId)).SalesArrangementId;
        var salesArrangementInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellation);

        // load Offer
        var offerId = salesArrangementInstance?.OfferId;
        var offerInstance = offerId.HasValue ? await _offerService.GetMortgageOfferDetail(offerId.Value, cancellation) : null;

        // load User
        var userInstance = await getUserInstance(caseInstance.CaseOwner?.UserId, cancellation);

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
            LoanPurposes = offerInstance!.SimulationInputs.LoanPurposes.Select(i => new LoanPurposeItem
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
            FirstAnnuityPaymentDate = offerInstance.SimulationResults.AnnuityPaymentsDateFrom,
            BranchConsultant = new BranchConsultantDto
            {
                BranchName = "Pobocka XXX",
                ConsultantName = userInstance?.UserInfo.DisplayName,
                Cpm = userInstance?.UserInfo.Cpm,
                Icp = userInstance?.UserInfo.Icp
            }
        };
    }

    private async Task<GetCaseParametersResponse> GetParamsAfterHandover(
        DomainServices.CaseService.Contracts.Case caseInstance,
        Dictionary<int, cCodebookService.ProductTypesResponse.Types.ProductTypeItem> productTypesById,
        Dictionary<int, cCodebookService.GenericCodebookResponse.Types.GenericCodebookItem> loanKindsById,
        Dictionary<int, cCodebookService.LoanPurposesResponse.Types.LoanPurposeItem> loanPurposesById,
        List<cCodebookService.StatementTypesResponse.Types.StatementTypeItem> statementTypes,
        List<cCodebookService.GenericCodebookResponse.Types.GenericCodebookItem> statementSubscriptionTypes,
        List<cCodebookService.StatementFrequenciesResponse.Types.StatementFrequencyItem> statementFrequencies,
        CancellationToken cancellation
        )
    {
        var mortgageData = (await _productService.GetMortgage(caseInstance.CaseId, cancellation)).Mortgage;

        var branchUser = await getUserInstance(mortgageData.BranchConsultantId, cancellation);
        var thirdPartyUser = await getUserInstance(mortgageData.ThirdPartyConsultantId, cancellation);

        var respone = new GetCaseParametersResponse
        {
            FirstAnnuityPaymentDate = mortgageData.FirstAnnuityPaymentDate,
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
            LoanPurposes = mortgageData.LoanPurposes.Select(i=> new LoanPurposeItem
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
            BranchConsultant = new BranchConsultantDto
            {
                BranchName = "Pobocka XXX",
                ConsultantName = branchUser?.UserInfo.DisplayName,
                Cpm = branchUser?.UserInfo.Cpm,
                Icp = branchUser?.UserInfo.Icp
            },
            ThirdPartyConsultant = new ThirdPartyConsultantDto
            {
                BranchName = "Spolecnost XXX",
                ConsultantName = thirdPartyUser?.UserInfo.DisplayName,
                Cpm = thirdPartyUser?.UserInfo.Cpm,
                Icp = thirdPartyUser?.UserInfo.Icp
            }
        };

        if (mortgageData.Statement is not null)
        {
            respone.Statement = new StatementDto
            {
                TypeId = mortgageData.Statement.TypeId,
                TypeName = statementTypes.FirstOrDefault(x => x.Id == mortgageData.Statement?.TypeId)?.Name,
                TypeShortName = statementTypes.FirstOrDefault(x => x.Id == mortgageData.Statement?.TypeId)?.ShortName,
                SubscriptionType = statementSubscriptionTypes.FirstOrDefault(x => x.Id == mortgageData.Statement?.SubscriptionTypeId)?.Name,
                Frequency = statementFrequencies.FirstOrDefault(x => x.Id == mortgageData.Statement?.FrequencyId)?.Name,
                EmailAddress1 = mortgageData.Statement?.EmailAddress1,
                EmailAddress2 = mortgageData.Statement?.EmailAddress2
            };
            if (mortgageData.Statement!.Address is not null)
            {
                respone.Statement.Address = (CIS.Foms.Types.Address)mortgageData.Statement!.Address!;
            }
        }

        return respone;
    }

    private async Task<DomainServices.UserService.Contracts.User?> getUserInstance(int? userId, CancellationToken cancellationToken)
    {
        if (!userId.HasValue)
            return null;

        try
        {
            return await _userService.GetUser(userId.Value, cancellationToken);
        }
        catch 
        {
            return null;
        }
    }
}
