using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;
using SharedTypes.Enums;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal sealed class UpdateMortgageHandler 
    : IRequestHandler<UpdateMortgageRequest>
{
    public async Task Handle(UpdateMortgageRequest request, CancellationToken cancellationToken)
    {
        // get data from other DS
        var productSA = await _salesArrangementService.GetProductSalesArrangement(request.ProductId, cancellationToken);
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(productSA.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetMortgageOfferDetail(salesArrangement.OfferId!.Value, cancellationToken);
        var customers = await _customerOnSAService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);

        var mortgageRequest = new MortgageRequest
        {
            PartnerId = customers
                .FirstOrDefault(t => t.CustomerRoleId == (int)CustomerRoles.Debtor)
                ?.CustomerIdentifiers
                .FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp)
                ?.IdentityId ?? 0,
            LoanContractNumber = salesArrangement.ContractNumber,
            MonthlyInstallment = offer.SimulationResults.LoanPaymentAmount,
            LoanAmount = (double?)offer.SimulationInputs.LoanAmount,
            InterestRate = (double?)offer.SimulationResults.LoanInterestRate,
            FixationPeriod = offer.SimulationInputs.FixedRatePeriod,
            LoanKind = offer.SimulationInputs.LoanKindId,
            Expected1stDrawDate = offer.SimulationInputs.ExpectedDateOfDrawing,
            FirstRequestSignDate = salesArrangement.FirstSignatureDate,
            InstallmentDay = offer.SimulationInputs.PaymentDay,
            LoanPurposes = offer.SimulationInputs.LoanPurposes?.Select(t => new global::ExternalServices.MpHome.V1.Contracts.LoanPurpose
            {
                Amount = (double)t.Sum,
                LoanPurposeId = t.LoanPurposeId
            }).ToList(),
            ProductCodeUv = offer.SimulationInputs.ProductTypeId,
            EstimatedDuePaymentDate = offer.SimulationResults.LoanDueDate,
            PcpInstId = salesArrangement.PcpId,
            FirstAnnuityInstallmentDate = offer.SimulationResults.AnnuityPaymentsDateFrom,
            Relationships = customers
                .Where(t => t.CustomerIdentifiers.Any(tt => tt.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp))
                .Select(t => new LoanContractRelationship
                {
                    ContractRelationshipType = (ContractRelationshipType)t.CustomerRoleId,
                    PartnerId = t.CustomerIdentifiers.First(tt => tt.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp).IdentityId
                })
                .ToList()
        };

        await _mpHomeClient.UpdateLoan(request.ProductId, mortgageRequest, cancellationToken);
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IMpHomeClient _mpHomeClient;

    public UpdateMortgageHandler(
        IMpHomeClient mpHomeClient, 
        ICustomerOnSAServiceClient customerOnSAService,
        ISalesArrangementServiceClient salesArrangementService, 
        IOfferServiceClient offerService)
    {

        _customerOnSAService = customerOnSAService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _mpHomeClient = mpHomeClient;
    }
}