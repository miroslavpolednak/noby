using _SA = DomainServices.SalesArrangementService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
using _Rip = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using CIS.Core;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

internal class GetCreditWorthinessHandler
    : IRequestHandler<GetCreditWorthinessRequest, GetCreditWorthinessResponse>
{
    public async Task<GetCreditWorthinessResponse> Handle(GetCreditWorthinessRequest request, CancellationToken cancellationToken)
    {
        // SA instance
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
        if (!saInstance.OfferId.HasValue)
            throw new CisNotFoundException(0, $"Offer instance not found for SA {saInstance.SalesArrangementId}");
        // case instance
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_Case.Case>(await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken));
        // offer instance
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.OfferService.Contracts.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken));
        // user instance
        var userInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.UserService.Contracts.User>(await _userService.GetUser(_userAccessor.User!.Id, cancellationToken));

#pragma warning disable CA1305 // Specify IFormatProvider
        var ripRequest = new _Rip.CreditWorthinessCalculateRequest
        {
            ResourceProcessId = offerInstance.ResourceProcessId,
            RiskBusinessCaseId = saInstance.RiskBusinessCaseId,
            UserIdentity = new()
            {
                IdentityScheme = ((CIS.Foms.Enums.UserIdentitySchemes)Convert.ToInt32(userInstance.UserIdentifiers[0].IdentityScheme)).GetAttribute<DisplayAttribute>()!.Name,
                IdentityId = userInstance.UserIdentifiers[0].Identity
            },
            Product = new()
            {
                ProductTypeId = caseInstance.Data.ProductTypeId,
                LoanDuration = offerInstance.SimulationResults.LoanDuration,
                LoanInterestRate = offerInstance.SimulationResults.LoanInterestRate,
                LoanAmount = Convert.ToInt32(offerInstance.SimulationResults.LoanAmount),
                LoanPaymentAmount = Convert.ToInt32((decimal?)offerInstance.SimulationResults.LoanPaymentAmount ?? 0M),
                FixedRatePeriod = offerInstance.SimulationInputs.FixedRatePeriod!.Value
            },
            Households = await _creditWorthinessHouseholdService.CreateHouseholds(request.SalesArrangementId, cancellationToken)
        };
#pragma warning restore CA1305 // Specify IFormatProvider

        var ripResult = ServiceCallResult.ResolveAndThrowIfError<_Rip.CreditWorthinessCalculateResponse>(await _creditWorthinessService.Calculate(ripRequest, cancellationToken));

        return new GetCreditWorthinessResponse
        {
            InstallmentLimit = Convert.ToInt32(ripResult.InstallmentLimit),
            MaxAmount = Convert.ToInt32(ripResult.MaxAmount),
            RemainsLivingAnnuity = Convert.ToInt32((decimal?)ripResult.RemainsLivingAnnuity ?? 0),
            RemainsLivingInst = Convert.ToInt32((decimal?)ripResult.RemainsLivingInst ?? 0),
            WorthinessResult = (CreditWorthinessResults)(int)ripResult.WorthinessResult,
            ResultReasonCode = ripResult.ResultReason?.Code,
            ResultReasonDescription = ripResult.ResultReason?.Description,
            Dti = ripResult.Dti,
            Dsti = ripResult.Dsti,
            LoanAmount = offerInstance.SimulationInputs.LoanAmount,
            LoanPaymentAmount = offerInstance.SimulationResults.LoanPaymentAmount
        };
    }

    private readonly CreditWorthinessHouseholdService _creditWorthinessHouseholdService;
    private readonly DomainServices.RiskIntegrationService.Clients.CreditWorthiness.V2.ICreditWorthinessServiceClient _creditWorthinessService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;

    public GetCreditWorthinessHandler(
        CreditWorthinessHouseholdService creditWorthinessHouseholdService,
        DomainServices.RiskIntegrationService.Clients.CreditWorthiness.V2.ICreditWorthinessServiceClient creditWorthinessService,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        DomainServices.UserService.Clients.IUserServiceClient userService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _creditWorthinessService = creditWorthinessService;
        _userService = userService;
        _caseService = caseService;
        _userAccessor = userAccessor;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _creditWorthinessHouseholdService = creditWorthinessHouseholdService;
    }
}
