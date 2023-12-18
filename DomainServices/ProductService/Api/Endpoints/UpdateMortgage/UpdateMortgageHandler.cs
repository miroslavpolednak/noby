using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal sealed class UpdateMortgageHandler 
    : IRequestHandler<UpdateMortgageRequest>
{
    public async Task Handle(UpdateMortgageRequest request, CancellationToken cancellationToken)
    {
        if (!await _repository.LoanExists(request.ProductId, cancellationToken))
            throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12001, request.ProductId);


        var productSA = await _salesArrangementService.GetProductSalesArrangement(request.ProductId, cancellationToken);
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(productSA.SalesArrangementId, cancellationToken);
        var offer = await _offerService.GetMortgageOfferDetail(salesArrangement.OfferId!.Value, cancellationToken);


        var mortgageRequest = new MortgageRequest
        {
            PartnerId = null,
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
                Amount = t.Sum,
                LoanPurposeId = t.LoanPurposeId
            }).ToList(),
            ProductCodeUv = offer.SimulationInputs.ProductTypeId,
            EstimatedDuePaymentDate = offer.SimulationResults.LoanDueDate,
            PcpInstId = salesArrangement.PcpId,
            FirstAnnuityInstallmentDate = offer.SimulationResults.AnnuityPaymentsDateFrom,
            Relationships = null
        };

        await _mpHomeClient.UpdateLoan(request.ProductId, mortgageRequest, cancellationToken);
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly LoanRepository _repository;
    private readonly IMpHomeClient _mpHomeClient;

    public UpdateMortgageHandler(
        LoanRepository repository, 
        IMpHomeClient mpHomeClient, 
        ICustomerOnSAServiceClient customerOnSAService,
        ISalesArrangementServiceClient salesArrangementService, 
        IOfferServiceClient offerService)
    {
        _customerOnSAService = customerOnSAService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _repository = repository;
        _mpHomeClient = mpHomeClient;
    }
}