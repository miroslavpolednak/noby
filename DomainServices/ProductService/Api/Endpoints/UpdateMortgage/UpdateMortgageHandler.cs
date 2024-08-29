using DomainServices.CaseService.Clients.v1;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using SharedTypes.Enums;
using SharedTypes.Extensions;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal sealed class UpdateMortgageHandler(
	IMpHomeClient _mpHomeClient,
	ICustomerOnSAServiceClient _customerOnSAService,
	ISalesArrangementServiceClient _salesArrangementService,
	IOfferServiceClient _offerService,
	ICaseServiceClient _caseService)
		: IRequestHandler<UpdateMortgageRequest>
{
    public async Task Handle(UpdateMortgageRequest request, CancellationToken cancellationToken)
    {
        // get data from other DS
        var productSA = (await _salesArrangementService.GetProductSalesArrangements(request.ProductId, cancellationToken)).FirstOrDefault() 
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001);

		var salesArrangement = await _salesArrangementService.GetSalesArrangement(productSA!.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(salesArrangement.OfferId!.Value, cancellationToken);
        var customers = await _customerOnSAService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        var caseInstance = await _caseService.ValidateCaseId(salesArrangement.CaseId, false, cancellationToken);

        var mortgageRequest = new MortgageRequest
        {
            PartnerId = customers
                .FirstOrDefault(t => t.CustomerRoleId == (int)EnumCustomerRoles.Debtor)
                ?.CustomerIdentifiers
                .GetMpIdentityOrDefault()?.IdentityId ?? 0,
            ConsultantId = caseInstance.OwnerUserId,
            LoanContractNumber = salesArrangement.ContractNumber,
            MonthlyInstallment = offer.MortgageOffer.SimulationResults.LoanPaymentAmount,
            LoanAmount = (double?)offer.MortgageOffer.SimulationInputs.LoanAmount,
            InterestRate = (double?)offer.MortgageOffer.SimulationResults.LoanInterestRateProvided,
            FixationPeriod = offer.MortgageOffer.SimulationInputs.FixedRatePeriod,
            LoanKind = offer.MortgageOffer.SimulationInputs.LoanKindId,
            Expected1stDrawDate = offer.MortgageOffer.SimulationInputs.ExpectedDateOfDrawing,
            FirstRequestSignDate = salesArrangement.FirstSignatureDate,
            InstallmentDay = offer.MortgageOffer.SimulationInputs.PaymentDay,
            LoanPurposes = offer.MortgageOffer.SimulationInputs.LoanPurposes?.Select(t => new global::ExternalServices.MpHome.V1.Contracts.LoanPurpose
            {
                Amount = (double)t.Sum,
                LoanPurposeId = t.LoanPurposeId
            }).ToList(),
            ProductCodeUv = offer.MortgageOffer.SimulationInputs.ProductTypeId,
            EstimatedDuePaymentDate = offer.MortgageOffer.SimulationResults.LoanDueDate,
            PcpInstId = salesArrangement.PcpId,
            FirstAnnuityInstallmentDate = offer.MortgageOffer.SimulationResults.AnnuityPaymentsDateFrom,
            Relationships = customers
                .Where(t => t.CustomerIdentifiers.HasMpIdentity())
                .Select(t => new LoanContractRelationship
                {
                    ContractRelationshipType = (ContractRelationshipType)t.CustomerRoleId,
                    PartnerId = t.CustomerIdentifiers.GetMpIdentity().IdentityId
                })
                .ToList()
        };

        await _mpHomeClient.UpdateLoan(request.ProductId, mortgageRequest, cancellationToken);
    }
}