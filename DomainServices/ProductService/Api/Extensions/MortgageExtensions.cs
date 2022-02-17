using DomainServices.ProductService.Contracts;
using ExternalServices.MpHome.V1._1.MpHomeWrapper;

namespace DomainServices.ProductService.Api;

internal static class MortgageExtensions
{

    /// <summary>
    /// Converts contract object MortgageData to MortgageRequest.
    /// </summary>
    public static MortgageRequest ToMortgageRequest(this MortgageData mortgage)
    {
        var request = new MortgageRequest
        {
            LoanType = LoanType.KBMortgage, // ProductTypeId
            PartnerId = mortgage.PartnerId,
            LoanContractNumber = mortgage.ContractNumber,
            LoanAmount = mortgage.LoanAmount,
            InterestRate = mortgage.LoanInterestRate,
            FixationPeriod = mortgage.FixedRatePeriod,
            MonthlyInstallment = mortgage.LoanPaymentAmount,
            LoanEventCode = mortgage.LoanActionCode,

            //TODO: add mapping (not specified so far)
            //CurrentAmount
            //DrawingMaxOn
            //VUP
            //Statement
        };

        return request;
    }


    /// <summary>
    /// Converts db entity Loan (table dbo.Uver) to contract object MortgageData.
    /// </summary>
    public static MortgageData ToMortgage(this Repositories.Entities.Loan eLoan, List<Repositories.Entities.Relationship> eRelationships)
    {
        var mortgage = new MortgageData
        {
            PartnerId = (int)eLoan.PartnerId,
            ContractNumber = eLoan.CisloSmlouvy,
            LoanAmount = eLoan.VyseUveru,
            LoanInterestRate = eLoan.RadnaSazba,
            FixedRatePeriod = eLoan.DelkaFixaceUrokoveSazby,
            // ProductTypeId = ???
            LoanPaymentAmount = eLoan.MesicniSplatka,
            LoanActionCode = eLoan.AkceUveruId,

            //TODO: add mapping (not specified so far)
            //CurrentAmount
            //DrawingMaxOn
            //VUP
            //Statement
        };

        if (eRelationships?.Count > 0)
        {
            mortgage.Relationships.AddRange(eRelationships.Select(e => e.ToRelationship()));
        }

        return mortgage;
    }

    
    /// <summary>
    /// Converts db entity Relationship (table dbo.VztahUver) to contract object Relationship .
    /// </summary>
    public static Relationship ToRelationship(this Repositories.Entities.Relationship eRelationship)
    {
        return new Relationship
        {
            PartnerId = (int)eRelationship.PartnerId,
            ContractRelationshipTypeId = eRelationship.VztahId
        };
    }
}

