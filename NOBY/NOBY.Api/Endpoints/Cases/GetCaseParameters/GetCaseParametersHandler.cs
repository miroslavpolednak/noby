using SharedTypes.Enums;
using SharedTypes.Types;
using NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

internal sealed class GetCaseParametersHandler : IRequestHandler<GetCaseParametersRequest, GetCaseParametersResponse>
{
    public async Task<GetCaseParametersResponse> Handle(GetCaseParametersRequest request, CancellationToken cancellationToken)
    {
        var response = new GetCaseParametersResponse
        {
            CaseParameters = new List<CaseParameters>()
        };

        // instance case
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // seznam produktovych SA
        var saList = await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken);
        var saInProgress = saList.FirstOrDefault(t => t.State is (int)SalesArrangementStates.NewArrangement or (int)SalesArrangementStates.InProgress);
        if (saInProgress is not null)
        {
            response.SalesArrangementInProgress = new SalesArrangementInProgressDto
            {
                SalesArrangementId = saInProgress.SalesArrangementId,
                ProductName = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == caseInstance.Data.ProductTypeId).Name,
            };
        }

        response.CaseParameters.AddRange(caseInstance.State == (int)CaseStates.InProgress
            ? await getCaseInProgress(caseInstance, saList, cancellationToken)
            : await getCaseFromSb(caseInstance, cancellationToken));

        return response;
    }

    private async Task<List<Dto.CaseParameters>> getCaseInProgress(
        DomainServices.CaseService.Contracts.Case caseInstance,
        List<GetProductSalesArrangementsResponse.Types.SalesArrangement> salesArrangements,
        CancellationToken cancellationToken)
    {
        // codebook
        var productTypes = await _codebookService.ProductTypes(cancellationToken);
        var loanKindTypes = await _codebookService.LoanKinds(cancellationToken);
        var loanPurposeTypes = await _codebookService.LoanPurposes(cancellationToken);

        List<Dto.CaseParameters> response = new();

        foreach (var sa in salesArrangements)
        {
            // get SA instance
            var salesArrangementInstance = await _salesArrangementService.GetSalesArrangement(sa.SalesArrangementId, cancellationToken);

            // get Offer
            var offerInstance = await _offerService.GetMortgageOfferDetail(salesArrangementInstance.OfferId!.Value, cancellationToken);

            // load User
            var caseOwnerOrig = await getUserInstance(caseInstance.CaseOwner?.UserId, cancellationToken);
            
            response.Add(new Dto.CaseParameters
            {
                ProductType = productTypes.FirstOrDefault(t => t.Id == offerInstance.SimulationInputs.ProductTypeId),
                ContractNumber = salesArrangementInstance.ContractNumber,
                LoanAmount = offerInstance.SimulationResults.LoanAmount,
                LoanInterestRate = offerInstance.SimulationResults.LoanInterestRateProvided,
                DrawingDateTo = offerInstance.SimulationResults.DrawingDateTo,
                LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount,
                LoanKind = loanKindTypes.FirstOrDefault(t => t.Id == offerInstance.SimulationInputs.LoanKindId),
                FixedRatePeriod = offerInstance.SimulationInputs.FixedRatePeriod,
                LoanPurposes = mapLoanPurposes(offerInstance.SimulationInputs.LoanPurposes, loanPurposeTypes).ToList(),
                ExpectedDateOfDrawing = salesArrangementInstance.Mortgage?.ExpectedDateOfDrawing,
                LoanDueDate = offerInstance.SimulationResults.LoanDueDate,
                PaymentDay = offerInstance.SimulationInputs.PaymentDay,
                FirstAnnuityPaymentDate = offerInstance.SimulationResults.AnnuityPaymentsDateFrom,
                CaseOwnerOrigUser = getCaseOwnerOrigUser(caseOwnerOrig, null)
            });
        }
        
        return response;
    }

    private async Task<List<Dto.CaseParameters>> getCaseFromSb(DomainServices.CaseService.Contracts.Case caseInstance, CancellationToken cancellationToken)
    {
        // codebook
        var productTypes = await _codebookService.ProductTypes(cancellationToken);
        var loanKindTypes = await _codebookService.LoanKinds(cancellationToken);
        var loanPurposeTypes = await _codebookService.LoanPurposes(cancellationToken);
        
        // load user
        var mortgageResponse = await _productService.GetMortgage(caseInstance.CaseId, cancellationToken);
        var mortgageData = mortgageResponse.Mortgage;
        var caseOwnerOrig = await getUserInstance(mortgageData.CaseOwnerUserOrigId, cancellationToken);
        var caseOwnerCurrent = await getUserInstance(mortgageData.CaseOwnerUserCurrentId, cancellationToken);

        List<Dto.CaseParameters> response = new();

        var parameters = new Dto.CaseParameters
        {
            FirstAnnuityPaymentDate = mortgageData.FirstAnnuityPaymentDate,
            ProductType = productTypes.First(t => t.Id == mortgageData.ProductTypeId),
            ContractNumber = mortgageData.ContractNumber,
            LoanAmount = mortgageData.LoanAmount,
            LoanInterestRate = mortgageData.LoanInterestRate,
            ContractSignedDate = mortgageData.ContractSignedDate,
            FixedRateValidTo = mortgageData.FixedRateValidTo,
            Principal = mortgageData.Principal,
            AvailableForDrawing = mortgageData.AvailableForDrawing,
            DrawingDateTo = mortgageData.DrawingDateTo,
            LoanPaymentAmount = mortgageData.LoanPaymentAmount,
            LoanKind = loanKindTypes.FirstOrDefault(t => t.Id == mortgageData.LoanKindId.GetValueOrDefault()),
            CurrentAmount = mortgageData.CurrentAmount,
            FixedRatePeriod = mortgageData.FixedRatePeriod,
            PaymentAccount = mortgageData.PaymentAccount.ToPaymentAccount(),
            CurrentOverdueAmount = mortgageData.CurrentOverdueAmount,
            AllOverdueFees = mortgageData.AllOverdueFees,
            OverdueDaysNumber = mortgageData.OverdueDaysNumber,
            LoanPurposes = mapLoanPurposes(mortgageData.LoanPurposes, loanPurposeTypes).ToList(),
            ExpectedDateOfDrawing = mortgageData.ExpectedDateOfDrawing,
            InterestInArrears = mortgageData.InterestInArrears,
            LoanDueDate = mortgageData.LoanDueDate,
            PaymentDay = mortgageData.PaymentDay,
            LoanInterestRateRefix = mortgageData.LoanInterestRateRefix,
            LoanInterestRateValidFromRefix = mortgageData.LoanInterestRateValidFromRefix,
            FixedRatePeriodRefix = mortgageData.FixedRatePeriodRefix,
            CaseOwnerOrigUser = getCaseOwnerOrigUser(caseOwnerOrig, caseOwnerCurrent)
        };
        

        if (mortgageData.Statement is not null)
        {
            var statementTypes = await _codebookService.StatementTypes(cancellationToken);
            var statementSubscriptionTypes = await _codebookService.StatementSubscriptionTypes(cancellationToken);
            var statementFrequencies = await _codebookService.StatementFrequencies(cancellationToken);

            parameters.Statement = new StatementDto
            {
                TypeId = mortgageData.Statement.TypeId,
                TypeShortName = statementTypes.FirstOrDefault(x => x.Id == mortgageData.Statement.TypeId)?.ShortName,
                SubscriptionType = statementSubscriptionTypes.FirstOrDefault(x => x.Id == mortgageData.Statement.SubscriptionTypeId)?.Name,
                Frequency = statementFrequencies.FirstOrDefault(x => x.Id == mortgageData.Statement?.FrequencyId)?.Name,
                EmailAddress1 = mortgageData.Statement.EmailAddress1,
                EmailAddress2 = mortgageData.Statement.EmailAddress2
            };

            if (mortgageData.Statement.Address is not null)
            {
                if (!string.IsNullOrWhiteSpace(mortgageData.Statement.Address.City) && mortgageData.Statement.Address.CountryId.HasValue)
                {
                    mortgageData.Statement.Address.SingleLineAddressPoint = await _customerService.FormatAddress(mortgageData.Statement.Address, cancellationToken);
                }

                parameters.Statement.Address = (SharedTypes.Types.Address)mortgageData.Statement.Address!;
            }
        }

        response.Add(parameters);
        return response;
    }

    private static IEnumerable<LoanPurposeItem> mapLoanPurposes(
        IEnumerable<DomainServices.OfferService.Contracts.LoanPurpose> loanPurposes,
        IEnumerable<DomainServices.CodebookService.Contracts.v1.LoanPurposesResponse.Types.LoanPurposeItem> loanPurposeTypes
    ) => loanPurposes.Select(l => mapLoanPurpose(l, loanPurposeTypes));

    private static LoanPurposeItem mapLoanPurpose(
        DomainServices.OfferService.Contracts.LoanPurpose loanPurpose,
        IEnumerable<DomainServices.CodebookService.Contracts.v1.LoanPurposesResponse.Types.LoanPurposeItem> loanPurposeTypes) => new()
    {
        LoanPurpose = loanPurposeTypes.First(t => t.Id == loanPurpose.LoanPurposeId),
        Sum = loanPurpose.Sum
    };
    
    private static IEnumerable<LoanPurposeItem> mapLoanPurposes(
        IEnumerable<DomainServices.ProductService.Contracts.LoanPurpose> loanPurposes,
        IEnumerable<DomainServices.CodebookService.Contracts.v1.LoanPurposesResponse.Types.LoanPurposeItem> loanPurposeTypes
    ) => loanPurposes.Select(l => mapLoanPurpose(l, loanPurposeTypes));

    private static LoanPurposeItem mapLoanPurpose(
        DomainServices.ProductService.Contracts.LoanPurpose loanPurpose,
        IEnumerable<DomainServices.CodebookService.Contracts.v1.LoanPurposesResponse.Types.LoanPurposeItem> loanPurposeTypes) => new()
    {
        LoanPurpose = loanPurposeTypes.First(t => t.Id == loanPurpose.LoanPurposeId),
        Sum = loanPurpose.Sum
    };
    
    private static CaseOwnerUserDto? getCaseOwnerOrigUser(
        DomainServices.UserService.Contracts.User? caseOwnerOrig,
        DomainServices.UserService.Contracts.User? caseOwnerCurrent)
    {
        var user = caseOwnerOrig ?? caseOwnerCurrent;

        if (user is null) return null;
        
        var identifiers = user?.UserIdentifiers ?? Enumerable.Empty<SharedTypes.GrpcTypes.UserIdentity>();
        
        return new CaseOwnerUserDto
        {
            BranchName = user?.UserInfo.PersonOrgUnitName ?? user?.UserInfo.DealerCompanyName,
            ConsultantName = user?.UserInfo.DisplayName,
            Cpm = user?.UserInfo.Cpm,
            Icp = user?.UserInfo.Icp,
            IsInternal = user?.UserInfo.IsInternal ?? false,
            UserIdentifiers = identifiers.Select(i => new UserIdentity(i.Identity, (int)i.IdentityScheme)).ToList()
        };
    }
    
    private async Task<DomainServices.UserService.Contracts.User?> getUserInstance(long? userId, CancellationToken cancellationToken)
    {
        if (!userId.HasValue)
            return null;

        try
        {
            return await _userService.GetUser(Convert.ToInt32(userId.Value), cancellationToken);
        }
        catch
        {
            return null;
        }
    }

    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;
    private readonly DomainServices.ProductService.Clients.IProductServiceClient _productService;
    private readonly DomainServices.OfferService.Clients.IOfferServiceClient _offerService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService;

    public GetCaseParametersHandler(
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService,
        DomainServices.ProductService.Clients.IProductServiceClient productService,
        DomainServices.OfferService.Clients.IOfferServiceClient offerService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        DomainServices.UserService.Clients.IUserServiceClient userService,
        DomainServices.CustomerService.Clients.ICustomerServiceClient customerService
        )
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _productService = productService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _userService = userService;
        _customerService = customerService;
    }
}
