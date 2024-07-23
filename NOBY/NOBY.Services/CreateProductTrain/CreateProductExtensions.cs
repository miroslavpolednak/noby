using DomainServices.OfferService.Contracts;
using DomainServices.ProductService.Contracts;

namespace NOBY.Services.CreateProductTrain;

internal static class CreateProductExtensions
{
    public static MortgageData ToDomainServiceRequest(this GetOfferResponse offerData, long partnerId, string contractNumber)
    {
        var model = new MortgageData
        {
            ContractNumber = contractNumber,
            PaymentDay = offerData.MortgageOffer.SimulationInputs.PaymentDay,
            ExpectedDateOfDrawing = offerData.MortgageOffer.SimulationInputs.ExpectedDateOfDrawing,
            FixedRatePeriod = offerData.MortgageOffer.SimulationInputs.FixedRatePeriod,
            LoanAmount = offerData.MortgageOffer.SimulationInputs.LoanAmount,
            LoanInterestRate = offerData.MortgageOffer.SimulationResults.LoanInterestRateProvided,
            ProductTypeId = offerData.MortgageOffer.SimulationInputs.ProductTypeId,
            LoanPaymentAmount = offerData.MortgageOffer.SimulationResults.LoanPaymentAmount,
            LoanKindId= offerData.MortgageOffer.SimulationInputs.LoanKindId,
            PartnerId = partnerId,
            LoanDueDate = offerData.MortgageOffer.SimulationResults.LoanDueDate,
            FirstAnnuityPaymentDate = offerData.MortgageOffer.SimulationResults.AnnuityPaymentsDateFrom
        };

        if (offerData.MortgageOffer.SimulationInputs.LoanPurposes is not null)
        {
            model.LoanPurposes.AddRange(offerData.MortgageOffer.SimulationInputs.LoanPurposes.Select(t => new DomainServices.ProductService.Contracts.LoanPurpose
            {
                LoanPurposeId = t.LoanPurposeId,
                Sum = t.Sum
            }));
        }

        model.Relationships.Add(new Relationship { ContractRelationshipTypeId = 1, PartnerId = partnerId });//TODO tady je vzdy 1? Jako Vlastnik?

        return model;
    }
}
