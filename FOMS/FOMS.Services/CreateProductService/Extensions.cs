using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Contracts;

namespace FOMS.Services.CreateProduct;

internal static class Extensions
{
    public static MortgageData ToDomainServiceRequest(this GetMortgageDataResponse offerData, int partnerId)
    {
        var model = new MortgageData
        {
            FixedRatePeriod = offerData.Inputs.FixedRatePeriod,
            LoanAmount = offerData.Inputs.LoanAmount,
            LoanInterestRate = offerData.Outputs.LoanInterestRate,
            ProductTypeId = offerData.Inputs.ProductTypeId,
            LoanPaymentAmount = offerData.Outputs.LoanPaymentAmount,
            PartnerId = partnerId
        };
        model.Relationships.Add(new Relationship { ContractRelationshipTypeId = 1, PartnerId = partnerId });//TODO tady je vzdy 1? Jako Vlastnik?

        return model;
    }
}

