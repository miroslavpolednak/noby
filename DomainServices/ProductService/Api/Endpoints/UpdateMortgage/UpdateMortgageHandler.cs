using DomainServices.CaseService.Clients.v1;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using SharedTypes.Enums;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal sealed class UpdateMortgageHandler 
    : IRequestHandler<UpdateMortgageRequest>
{
    public async Task Handle(UpdateMortgageRequest request, CancellationToken cancellationToken)
    {
        // get data from other DS
        var productSA = (await _salesArrangementService.GetProductSalesArrangements(request.ProductId, cancellationToken)).FirstOrDefault();
        if (productSA is null)
        {
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001);
        }

        var salesArrangement = await _salesArrangementService.GetSalesArrangement(productSA!.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetOffer(salesArrangement.OfferId!.Value, cancellationToken);
        var customers = await _customerOnSAService.GetCustomerList(salesArrangement.SalesArrangementId, cancellationToken);
        var caseInstance = await _caseService.ValidateCaseId(salesArrangement.CaseId, false, cancellationToken);

        var mortgageRequest = new MortgageRequest
        {
            PartnerId = customers
                .FirstOrDefault(t => t.CustomerRoleId == (int)CustomerRoles.Debtor)
                ?.CustomerIdentifiers
                .FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp)
                ?.IdentityId ?? 0,
            LoanType = LoanType.KBMortgage,
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

    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IMpHomeClient _mpHomeClient;

    public UpdateMortgageHandler(
        IMpHomeClient mpHomeClient,
        ICustomerOnSAServiceClient customerOnSAService,
        ISalesArrangementServiceClient salesArrangementService,
        IOfferServiceClient offerService,
        ICaseServiceClient caseService)
    {
        _customerOnSAService = customerOnSAService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _mpHomeClient = mpHomeClient;
        _caseService = caseService;
    }
}