using CIS.Core.Security;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

internal sealed class GetCaseParametersHandler(
    GetCaseParametersMapper _mapper,
    DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService,
    DomainServices.ProductService.Clients.IProductServiceClient _productService,
    DomainServices.OfferService.Clients.v1.IOfferServiceClient _offerService,
    DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService,
    DomainServices.UserService.Clients.IUserServiceClient _userService,
    DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService,
    ICurrentUserAccessor _currentUserAccessor) 
    : IRequestHandler<GetCaseParametersRequest, CasesGetCaseParametersResponse>
{
    public async Task<CasesGetCaseParametersResponse> Handle(GetCaseParametersRequest request, CancellationToken cancellationToken)
    {
        var response = new CasesGetCaseParametersResponse
        {
            CaseParameters = []
        };

        // instance case
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // seznam produktovych SA
        var saList = await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken);
        var saInProgress = saList.FirstOrDefault(t => _allowedSAStates.Contains(t.State));
        if (saInProgress is not null)
        {
            response.SalesArrangementInProgress = new()
            {
                SalesArrangementId = saInProgress.SalesArrangementId,
                ProductName = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == caseInstance.Data.ProductTypeId).Name,
            };
        }

        response.CaseParameters.AddRange(caseInstance.State == (int)EnumCaseStates.InProgress
            ? await getCaseInProgress(caseInstance, saList, cancellationToken)
            : await getCaseFromSb(caseInstance, cancellationToken));

        return response;
    }

    private async Task<List<CasesGetCaseParametersCaseParameters>> getCaseInProgress(
        DomainServices.CaseService.Contracts.Case caseInstance,
        List<GetProductSalesArrangementsResponse.Types.SalesArrangement> salesArrangements,
        CancellationToken cancellationToken)
    {
        List<CasesGetCaseParametersCaseParameters> response = [];

        foreach (var sa in salesArrangements)
        {
            // get SA instance
            var salesArrangementInstance = await _salesArrangementService.GetSalesArrangement(sa.SalesArrangementId, cancellationToken);

            // get Offer
            var offerInstance = await _offerService.GetOffer(salesArrangementInstance.OfferId!.Value, cancellationToken);

            // load User
            var caseOwnerOrig = await getUserInstance(caseInstance.CaseOwner?.UserId, cancellationToken);
            
            response.Add(new CasesGetCaseParametersCaseParameters
            {
                ProductType = await _mapper.GetProductType(offerInstance.MortgageOffer.SimulationInputs.ProductTypeId, cancellationToken),
                ContractNumber = salesArrangementInstance.ContractNumber,
                LoanAmount = offerInstance.MortgageOffer.SimulationResults.LoanAmount,
                LoanInterestRate = offerInstance.MortgageOffer.SimulationResults.LoanInterestRateProvided,
                DrawingDateTo = offerInstance.MortgageOffer.SimulationResults.DrawingDateTo,
                LoanPaymentAmount = offerInstance.MortgageOffer.SimulationResults.LoanPaymentAmount,
                LoanKind = await _mapper.GetLoanKind(offerInstance.MortgageOffer.SimulationInputs.LoanKindId, cancellationToken),
                FixedRatePeriod = offerInstance.MortgageOffer.SimulationInputs.FixedRatePeriod,
                LoanPurposes = await _mapper.GetLoanPurposes(offerInstance.MortgageOffer.SimulationInputs.LoanPurposes, cancellationToken),
                ExpectedDateOfDrawing = salesArrangementInstance.Mortgage?.ExpectedDateOfDrawing,
                LoanDueDate = offerInstance.MortgageOffer.SimulationResults.LoanDueDate,
                PaymentDay = offerInstance.MortgageOffer.SimulationInputs.PaymentDay,
                FirstAnnuityPaymentDate = offerInstance.MortgageOffer.SimulationResults.AnnuityPaymentsDateFrom,
                CaseOwnerOrigUser = _mapper.GetCaseOwnerOrigUser(caseOwnerOrig, null)
            });
        }
        
        return response;
    }

    private async Task<List<CasesGetCaseParametersCaseParameters>> getCaseFromSb(DomainServices.CaseService.Contracts.Case caseInstance, CancellationToken cancellationToken)
    {
        // load user
        var mortgageResponse = await _productService.GetMortgage(caseInstance.CaseId, cancellationToken);
        var mortgageData = mortgageResponse.Mortgage;
        var caseOwnerOrig = await getUserInstance(mortgageData.CaseOwnerUserOrigId, cancellationToken);
        var caseOwnerCurrent = await getUserInstance(mortgageData.CaseOwnerUserCurrentId, cancellationToken);

        List<CasesGetCaseParametersCaseParameters> response = [];

        var parameters = new CasesGetCaseParametersCaseParameters
        {
            FirstAnnuityPaymentDate = mortgageData.FirstAnnuityPaymentDate,
            ProductType = await _mapper.GetProductType(mortgageData.ProductTypeId, cancellationToken),
            ContractNumber = mortgageData.ContractNumber,
            LoanAmount = mortgageData.LoanAmount,
            LoanInterestRate = mortgageData.LoanInterestRate,
            ContractSignedDate = mortgageData.ContractSignedDate,
            FixedRateValidTo = mortgageData.FixedRateValidTo,
            Principal = mortgageData.Principal,
            AvailableForDrawing = mortgageData.AvailableForDrawing,
            DrawingDateTo = mortgageData.DrawingDateTo,
            LoanPaymentAmount = mortgageData.LoanPaymentAmount,
            LoanKind = await _mapper.GetLoanKind(mortgageData.LoanKindId.GetValueOrDefault(), cancellationToken),
            CurrentAmount = mortgageData.CurrentAmount,
            FixedRatePeriod = mortgageData.FixedRatePeriod,
            PaymentAccount = mortgageData.PaymentAccount is null ? null : new SharedTypesBankAccount
            {
                AccountBankCode = mortgageData.PaymentAccount.BankCode,
                AccountNumber = mortgageData.PaymentAccount.Number,
                AccountPrefix = mortgageData.PaymentAccount.Prefix
            },
            CurrentOverdueAmount = mortgageData.CurrentOverdueAmount,
            AllOverdueFees = mortgageData.AllOverdueFees,
            OverdueDaysNumber = mortgageData.OverdueDaysNumber,
            LoanPurposes = await _mapper.GetLoanPurposes(mortgageData.LoanPurposes, cancellationToken),
            ExpectedDateOfDrawing = mortgageData.ExpectedDateOfDrawing,
            InterestInArrears = mortgageData.InterestInArrears,
            LoanDueDate = mortgageData.LoanDueDate,
            PaymentDay = mortgageData.PaymentDay,
            CaseOwnerOrigUser = _mapper.GetCaseOwnerOrigUser(caseOwnerOrig, caseOwnerCurrent)
        };

        if (_currentUserAccessor.HasPermission(UserPermissions.REFINANCING_Manage))
        {
            parameters.LoanInterestRateRefixation = mortgageData.Refixation?.LoanInterestRate;
            parameters.LoanInterestRateValidFromRefixation = ((DateOnly?)mortgageData.FixedRateValidTo)?.AddDays(1);
            parameters.FixedRatePeriodRefixation = mortgageData.Refixation?.FixedRatePeriod;
        }

        if (mortgageData.Statement is not null)
        {
            var statementTypes = await _codebookService.StatementTypes(cancellationToken);
            var statementFrequencies = await _codebookService.StatementFrequencies(cancellationToken);

            parameters.Statement = new CasesGetCaseParametersStatement
            {
                TypeId = mortgageData.Statement.TypeId,
                TypeShortName = statementTypes.FirstOrDefault(x => x.Id == mortgageData.Statement.TypeId)?.ShortName,
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

                parameters.Statement.Address = mortgageData.Statement.Address!;
            }
        }

        response.Add(parameters);
        return response;
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

    private static readonly int[] _allowedSAStates =
    [
        (int)SharedTypes.Enums.EnumSalesArrangementStates.InSigning,
        (int)SharedTypes.Enums.EnumSalesArrangementStates.ToSend,
        (int)SharedTypes.Enums.EnumSalesArrangementStates.NewArrangement,
        (int)SharedTypes.Enums.EnumSalesArrangementStates.InProgress 
    ];
}
