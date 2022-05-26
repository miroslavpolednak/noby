using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Contracts;

namespace FOMS.Api.Notifications.Handlers;

internal static class CreateProductExtensions
{
    public static MortgageData ToDomainServiceRequest(this GetMortgageOfferResponse offerData, int partnerId)
    {
        var model = new MortgageData
        {
            FixedRatePeriod = offerData.SimulationInputs.FixedRatePeriod,
            LoanAmount = offerData.SimulationInputs.LoanAmount,
            LoanInterestRate = offerData.SimulationResults.LoanInterestRate,
            ProductTypeId = offerData.SimulationInputs.ProductTypeId,
            LoanPaymentAmount = offerData.SimulationResults.LoanPaymentAmount,
            PartnerId = partnerId
        };
        model.Relationships.Add(new Relationship { ContractRelationshipTypeId = 1, PartnerId = partnerId });//TODO tady je vzdy 1? Jako Vlastnik?

        return model;
    }
}
