using _Rip = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using CIS.Core;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

internal sealed class GetCreditWorthinessHandler(
    CreditWorthinessHouseholdService _creditWorthinessHouseholdService,
    DomainServices.RiskIntegrationService.Clients.CreditWorthiness.V2.ICreditWorthinessServiceClient _creditWorthinessService,
    CIS.Core.Security.ICurrentUserAccessor _userAccessor,
    DomainServices.UserService.Clients.v1.IUserServiceClient _userService,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService,
    DomainServices.OfferService.Clients.v1.IOfferServiceClient _offerService,
    DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService)
        : IRequestHandler<GetCreditWorthinessRequest, SalesArrangementGetCreditWorthinessResponse>
{
    public async Task<SalesArrangementGetCreditWorthinessResponse> Handle(GetCreditWorthinessRequest request, CancellationToken cancellationToken)
    {
        // SA instance
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        if (!saInstance.OfferId.HasValue)
            throw new CisNotFoundException(0, $"Offer instance not found for SA {saInstance.SalesArrangementId}");
        // case instance
        var caseInstance = await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken);
        // offer instance
        var offerInstance = await _offerService.GetOffer(saInstance.OfferId!.Value, cancellationToken);
        // user instance
        var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);

#pragma warning disable CA1305 // Specify IFormatProvider
        var ripRequest = new _Rip.CreditWorthinessCalculateRequest
        {
            ResourceProcessId = offerInstance.Data.ResourceProcessId,
            RiskBusinessCaseId = saInstance.RiskBusinessCaseId,
            UserIdentity = new()
            {
                IdentityScheme = ((SharedTypes.Enums.UserIdentitySchemes)Convert.ToInt32(userInstance.UserIdentifiers[0].IdentityScheme)).GetAttribute<DisplayAttribute>()!.Name,
                IdentityId = userInstance.UserIdentifiers[0].Identity
            },
            Product = new()
            {
                ProductTypeId = caseInstance.Data.ProductTypeId,
                LoanDuration = offerInstance.MortgageOffer.SimulationResults.LoanDuration,
                LoanInterestRate = offerInstance.MortgageOffer.SimulationResults.LoanInterestRateProvided,
                LoanAmount = Convert.ToInt32(offerInstance.MortgageOffer.SimulationResults.LoanAmount),
                LoanPaymentAmount = Convert.ToInt32((decimal?)offerInstance.MortgageOffer.SimulationResults.LoanPaymentAmount ?? 0M),
                FixedRatePeriod = offerInstance.MortgageOffer.SimulationInputs.FixedRatePeriod!.Value
            },
            Households = await _creditWorthinessHouseholdService.CreateHouseholds(request.SalesArrangementId, cancellationToken)
        };
#pragma warning restore CA1305 // Specify IFormatProvider

        var ripResult = await _creditWorthinessService.Calculate(ripRequest, cancellationToken);

        return new SalesArrangementGetCreditWorthinessResponse
        {
            InstallmentLimit = Convert.ToInt32(ripResult.InstallmentLimit),
            MaxAmount = Convert.ToInt32(ripResult.MaxAmount),
            RemainsLivingAnnuity = Convert.ToInt32((decimal?)ripResult.RemainsLivingAnnuity ?? 0),
            RemainsLivingInst = Convert.ToInt32((decimal?)ripResult.RemainsLivingInst ?? 0),
            WorthinessResult = (SalesArrangementGetCreditWorthinessResponseWorthinessResult)(int)ripResult.WorthinessResult,
            ResultReasonCode = ripResult.ResultReason?.Code,
            ResultReasonDescription = ripResult.ResultReason?.Description,
            Dti = ripResult.Dti,
            Dsti = ripResult.Dsti,
            LoanAmount = offerInstance.MortgageOffer.SimulationInputs.LoanAmount,
            LoanPaymentAmount = offerInstance.MortgageOffer.SimulationResults.LoanPaymentAmount
        };
    }
}
