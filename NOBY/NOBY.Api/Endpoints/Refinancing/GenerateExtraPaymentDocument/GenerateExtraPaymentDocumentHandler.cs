using System.Globalization;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.SbWebApi.Dto.Refinancing;
using ExternalServices.SbWebApi.V1;
using NOBY.Services.MortgageRefinancing;
using PublicHoliday;
using SharedTypes.GrpcTypes;
using _contract = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

internal sealed class GenerateExtraPaymentDocumentHandler(
    ICodebookServiceClient _codebookService,
    ISalesArrangementServiceClient _salesArrangementService,
    IOfferServiceClient _offerService,
    IProductServiceClient _productService,
    ICustomerServiceClient _customerService,
    ISbWebApiClient _sbWebApi,
    MortgageRefinancingDocumentService _refinancingDocumentService) : IRequestHandler<RefinancingGenerateExtraPaymentDocumentRequest>
{
    public async Task Handle(RefinancingGenerateExtraPaymentDocumentRequest request, CancellationToken cancellationToken)
    {
        await ValidateHandoverTypeDetail(request, cancellationToken);

        var salesArrangement = await _refinancingDocumentService.LoadAndValidateSA(request.SalesArrangementId, SalesArrangementTypes.MortgageExtraPayment, cancellationToken);
        var offer = await LoadAndValidateOffer(salesArrangement.CaseId, salesArrangement.OfferId!.Value, cancellationToken);
        
        var offerIndividualPrice = new MortgageRefinancingIndividualPrice(null, (decimal?)offer.MortgageExtraPayment.BasicParameters.FeeAmountDiscount);

        if (offerIndividualPrice.HasIndividualPrice && !await _refinancingDocumentService.IsIndividualPriceValid(salesArrangement, offerIndividualPrice, cancellationToken))
            throw new NobyValidationException(90048);

        var customerDetail = await LoadAndValidateClient(salesArrangement.CaseId, request.ClientKbId!.Value, cancellationToken);

        await UpdateSaParams(request, salesArrangement, customerDetail, cancellationToken);

        await GenerateCalculationDocuments(request, salesArrangement, offer, offerIndividualPrice.HasIndividualPrice, cancellationToken);

        await _salesArrangementService.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, EnumSalesArrangementStates.Finished, cancellationToken);
    }

    private async Task ValidateHandoverTypeDetail(RefinancingGenerateExtraPaymentDocumentRequest request, CancellationToken cancellationToken)
    {
        var handoverTypeDetail = (await _codebookService.HandoverTypeDetails(cancellationToken)).SingleOrDefault(s => s.Id == request.HandoverTypeDetailId);

        if (handoverTypeDetail is null)
            throw new NobyValidationException(90032);
    }

    private async Task<GetOfferResponse> LoadAndValidateOffer(long caseId, int offerId, CancellationToken cancellationToken)
    {
        var offer = await _offerService.GetOffer(offerId, cancellationToken);

        if (offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentDate < new CzechRepublicPublicHoliday().NextWorkingDay(DateTime.Now, 3).AddDays(1))
            throw new NobyValidationException(90055);

        var simulationRequest = new SimulateMortgageExtraPaymentRequest
        {
            CaseId = caseId,
            SimulationInputs = new MortgageExtraPaymentSimulationInputs
            {
                ExtraPaymentDate = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentDate,
                ExtraPaymentAmount = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentAmount,
                ExtraPaymentReasonId = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentReasonId,
                IsExtraPaymentFullyRepaid = offer.MortgageExtraPayment.SimulationInputs.IsExtraPaymentFullyRepaid
            },
            BasicParameters = new MortgageExtraPaymentBasicParameters
            {
                FeeAmountDiscount = offer.MortgageExtraPayment.BasicParameters.FeeAmountDiscount
            }
        };

        var simulation = await _offerService.SimulateMortgageExtraPayment(simulationRequest, cancellationToken);

        var oldSimulationData = new OfferSimulationData(offer.MortgageExtraPayment.SimulationResults);
        var newSimulationData = new OfferSimulationData(simulation.SimulationResults);

        if (!oldSimulationData.Equals(newSimulationData))
            throw new NobyValidationException(90055);

        return offer;
    }

    private async Task<DomainServices.CustomerService.Contracts.Customer> LoadAndValidateClient(long caseId, long clientKbId, CancellationToken cancellationToken)
    {
        var customersOnProduct = await _productService.GetCustomersOnProduct(caseId, cancellationToken);

        if (!customersOnProduct.Customers.Any(c => c.CustomerIdentifiers.Any(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb && i.IdentityId == clientKbId)))
            throw new NobyValidationException(90032);

        return await _customerService.GetCustomerDetail(new Identity(clientKbId, IdentitySchemes.Kb), cancellationToken);
    }

    private async Task UpdateSaParams(RefinancingGenerateExtraPaymentDocumentRequest request, _contract.SalesArrangement salesArrangement, DomainServices.CustomerService.Contracts.Customer customerDetail, CancellationToken cancellationToken)
    {
        salesArrangement.ExtraPayment.HandoverTypeDetailId = request.HandoverTypeDetailId;
        salesArrangement.ExtraPayment.Client = new SalesArrangementParametersExtraPayment.Types.SalesArrangementParametersExtraPaymentClient
        {
            KBId = request.ClientKbId!.Value,
            FirstName = customerDetail.NaturalPerson.FirstName,
            LastName = customerDetail.NaturalPerson.LastName,
        };

        var saRequest = new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            ExtraPayment = salesArrangement.ExtraPayment
        };

        await _salesArrangementService.UpdateSalesArrangementParameters(saRequest, cancellationToken);
    }

    private async Task GenerateCalculationDocuments(RefinancingGenerateExtraPaymentDocumentRequest request, _contract.SalesArrangement salesArrangement, GetOfferResponse offer, bool hasIndividualPricing, CancellationToken cancellationToken)
    {
        var handoverTypeDetails = await _codebookService.HandoverTypeDetails(cancellationToken);

        await _sbWebApi.GenerateCalculationDocuments(new GenerateCalculationDocumentsRequest
        {
            CaseId = salesArrangement.CaseId,
            IsExtraPaymentFullyRepaid = offer.MortgageExtraPayment.SimulationInputs.IsExtraPaymentFullyRepaid,
            ExtraPaymentDate = offer.MortgageExtraPayment.SimulationInputs.ExtraPaymentDate,
            ClientKbId = request.ClientKbId!.Value,
            ExtraPaymentAmount = offer.MortgageExtraPayment.SimulationResults.ExtraPaymentAmount - ((decimal?)offer.MortgageExtraPayment.BasicParameters.FeeAmountDiscount ?? 0m),
            FeeAmount = offer.MortgageExtraPayment.SimulationResults.FeeAmount -  ((decimal?)offer.MortgageExtraPayment.BasicParameters.FeeAmountDiscount ?? 0m),
            PrincipalAmount = offer.MortgageExtraPayment.SimulationResults.PrincipalAmount,
            InterestAmount = offer.MortgageExtraPayment.SimulationResults.InterestAmount,
            OtherUnpaidFees = offer.MortgageExtraPayment.SimulationResults.OtherUnpaidFees,
            InterestOnLate = offer.MortgageExtraPayment.SimulationResults.InterestOnLate,
            InterestCovid = offer.MortgageExtraPayment.SimulationResults.InterestCovid,
            IsLoanOverdue = offer.MortgageExtraPayment.SimulationResults.IsLoanOverdue,
			IsInstallmentReduced = offer.MortgageExtraPayment.SimulationResults.IsInstallmentReduced,
            NewMaturityDate = offer.MortgageExtraPayment.SimulationResults.NewMaturityDate,
            NewPaymentAmount = offer.MortgageExtraPayment.SimulationResults.NewPaymentAmount,
            HandoverTypeDetailCode = int.Parse(handoverTypeDetails.First(h => h.Id == request.HandoverTypeDetailId).Code, CultureInfo.InvariantCulture),
            IndividualPricing = hasIndividualPricing
        }, cancellationToken);
    }
}