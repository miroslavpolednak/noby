using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Contracts;

namespace NOBY.Api.Notifications.Handlers;

internal static class CreateProductExtensions
{
    public static MortgageData ToDomainServiceRequest(this GetMortgageOfferResponse offerData, long partnerId, string contractNumber)
    {
        var model = new MortgageData
        {
            ContractNumber = contractNumber,
            PaymentDay = offerData.SimulationInputs.PaymentDay,
            ExpectedDateOfDrawing = offerData.SimulationInputs.ExpectedDateOfDrawing,
            FixedRatePeriod = offerData.SimulationInputs.FixedRatePeriod,
            LoanAmount = offerData.SimulationInputs.LoanAmount,
            LoanInterestRate = offerData.SimulationResults.LoanInterestRate,
            ProductTypeId = offerData.SimulationInputs.ProductTypeId,
            LoanPaymentAmount = offerData.SimulationResults.LoanPaymentAmount,
            LoanKindId= offerData.SimulationInputs.LoanKindId,
            PartnerId = partnerId
        };

        if (offerData.SimulationInputs.LoanPurposes is not null)
        {
            model.LoanPurposes.AddRange(offerData.SimulationInputs.LoanPurposes.Select(t => new DomainServices.ProductService.Contracts.LoanPurpose
            {
                LoanPurposeId = t.LoanPurposeId,
                Sum = t.Sum
            }));
        }

        model.Relationships.Add(new Relationship { ContractRelationshipTypeId = 1, PartnerId = partnerId });//TODO tady je vzdy 1? Jako Vlastnik?

        return model;
    }
}
