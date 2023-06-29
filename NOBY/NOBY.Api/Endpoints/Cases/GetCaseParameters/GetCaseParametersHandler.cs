using CIS.Core.Security;
using CIS.Foms.Enums;
using DomainServices.CodebookService.Contracts.v1;
using NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;
using System.Data;
using System.Threading;
using cCodebookService = DomainServices.CodebookService.Contracts.v1;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

internal sealed class GetCaseParametersHandler
    : IRequestHandler<GetCaseParametersRequest, GetCaseParametersResponse>
{
    public async Task<GetCaseParametersResponse> Handle(GetCaseParametersRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        if (caseInstance.CaseOwner.UserId != _currentUser.User!.Id && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException();
        }

        if (caseInstance.State == (int)CaseStates.InProgress)
        {
            return await getCaseInProgress(caseInstance, cancellationToken);
        }
        else
        {
            return await getCaseFromSb(caseInstance, cancellationToken);
        }
    }

    private async Task<GetCaseParametersResponse> getCaseInProgress(DomainServices.CaseService.Contracts.Case caseInstance, CancellationToken cancellationToken)
    {
        // get product SAId
        var salesArrangementId = await _salesArrangementService.GetProductSalesArrangement(caseInstance.CaseId, cancellationToken);
        // get SA instance
        var salesArrangementInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId.SalesArrangementId, cancellationToken);

        // get Offer
        var offerInstance = await _offerService.GetMortgageOfferDetail(salesArrangementInstance.OfferId!.Value, cancellationToken);

        // load User
        var userInstance = await getUserInstance(caseInstance.CaseOwner?.UserId, cancellationToken);
        var loanPurposes = await _codebookService.LoanPurposes(cancellationToken);

        return new GetCaseParametersResponse
        {
            ProductType = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == offerInstance.SimulationInputs.ProductTypeId),
            ContractNumber = salesArrangementInstance.ContractNumber,
            LoanAmount = offerInstance.SimulationResults.LoanAmount,
            LoanInterestRate = offerInstance.SimulationResults.LoanInterestRateProvided,
            DrawingDateTo = offerInstance.SimulationResults.DrawingDateTo,
            LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount,
            LoanKind = (await _codebookService.LoanKinds(cancellationToken)).First(t => t.Id == offerInstance.SimulationInputs.LoanKindId),
            FixedRatePeriod = offerInstance.SimulationInputs.FixedRatePeriod,
            LoanPurposes = offerInstance.SimulationInputs.LoanPurposes.Select(i => new LoanPurposeItem
            {
                LoanPurpose = loanPurposes.First(t => t.Id == i.LoanPurposeId),
                Sum = i.Sum
            }).ToList(),
            ExpectedDateOfDrawing = salesArrangementInstance.Mortgage?.ExpectedDateOfDrawing,
            LoanDueDate = offerInstance.SimulationResults.LoanDueDate,
            PaymentDay = offerInstance.SimulationInputs.PaymentDay,
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

    private async Task<GetCaseParametersResponse> getCaseFromSb(DomainServices.CaseService.Contracts.Case caseInstance, CancellationToken cancellationToken)
    {
        var mortgageData = (await _productService.GetMortgage(caseInstance.CaseId, cancellationToken)).Mortgage;
        var loanPurposes = await _codebookService.LoanPurposes(cancellationToken);

        var branchUser = await getUserInstance(mortgageData.BranchConsultantId, cancellationToken);
        var thirdPartyUser = await getUserInstance(mortgageData.ThirdPartyConsultantId, cancellationToken);

        var respone = new GetCaseParametersResponse
        {
            FirstAnnuityPaymentDate = mortgageData.FirstAnnuityPaymentDate,
            ProductType = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == mortgageData.ProductTypeId),
            ContractNumber = mortgageData.ContractNumber,
            LoanAmount = mortgageData.LoanAmount,
            LoanInterestRate = mortgageData.LoanInterestRate,
            ContractSignedDate = mortgageData.ContractSignedDate,
            FixedRateValidTo = mortgageData.FixedRateValidTo,
            Principal = mortgageData.Principal,
            AvailableForDrawing = mortgageData.AvailableForDrawing,
            DrawingDateTo = mortgageData.DrawingDateTo,
            LoanPaymentAmount = mortgageData.LoanPaymentAmount,
            LoanKind = mortgageData.LoanKindId.HasValue ? (await _codebookService.LoanKinds(cancellationToken)).First(t => t.Id == mortgageData.LoanKindId.Value) : null,
            CurrentAmount = mortgageData.CurrentAmount,
            FixedRatePeriod = mortgageData.FixedRatePeriod,
            PaymentAccount = mortgageData.PaymentAccount.ToPaymentAccount(),
            CurrentOverdueAmount = mortgageData.CurrentOverdueAmount,
            AllOverdueFees = mortgageData.AllOverdueFees,
            OverdueDaysNumber = mortgageData.OverdueDaysNumber,
            LoanPurposes = mortgageData.LoanPurposes.Select(i => new LoanPurposeItem
            {
                LoanPurpose = loanPurposes.First(t => t.Id == i.LoanPurposeId),
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
                TypeShortName = (await _codebookService.StatementTypes(cancellationToken)).FirstOrDefault(x => x.Id == mortgageData.Statement?.TypeId)?.ShortName,
                SubscriptionType = (await _codebookService.StatementSubscriptionTypes(cancellationToken)).FirstOrDefault(x => x.Id == mortgageData.Statement?.SubscriptionTypeId)?.Name,
                Frequency = (await _codebookService.StatementFrequencies(cancellationToken)).FirstOrDefault(x => x.Id == mortgageData.Statement?.FrequencyId)?.Name,
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

    private readonly ICurrentUserAccessor _currentUser;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;
    private readonly DomainServices.ProductService.Clients.IProductServiceClient _productService;
    private readonly DomainServices.OfferService.Clients.IOfferServiceClient _offerService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;

    public GetCaseParametersHandler(
        ICurrentUserAccessor currentUser,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService,
        DomainServices.ProductService.Clients.IProductServiceClient productService,
        DomainServices.OfferService.Clients.IOfferServiceClient offerService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService,
        DomainServices.UserService.Clients.IUserServiceClient userService
        )
    {
        _currentUser = currentUser;
        _codebookService = codebookService;
        _caseService = caseService;
        _productService = productService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _userService = userService;
    }
}
