using DomainServices.ProductService.Contracts;
using DomainServices.ProductService.Api.Repositories.Entities;
using ExternalServices.MpHome.V1._1.MpHomeWrapper;

namespace DomainServices.ProductService.Api;

internal static class MortgageExtensions
{
    /// <summary>
    /// Converts db entity to contract MortgageData object.
    /// </summary>
    public static MortgageData ToMortgage(this Loan loan)
    {
        var data = new MortgageData
        {
            PartnerId = loan.PartnerId,
            CaseId = loan.Id, //???
            ContractNumber = loan.CisloSmlouvy,
            LoanAmount = loan.VyseUveru, //.HasValue ? Decimal.ToDouble(loan.VyseUveru.Value) : null,
            LoanInterestRate = loan.RadnaSazba, //.HasValue ? Decimal.ToDouble(loan.RadnaSazba.Value) : null,
            FixedRatePeriod = loan.DelkaFixaceUrokoveSazby,
            // ProductTypeId = ???
            LoanPaymentAmount = loan.MesicniSplatka, //.HasValue ? Decimal.ToDouble(loan.MesicniSplatka.Value) : null,
            LoanActionCode = loan.AkceUveruId,

            //TODO: add mapping (not specified so far)
            //CurrentAmount
            //DrawingMaxOn
            //VUP
            //Statement
        };

        return data;
    }

    /// <summary>
    /// Converts MortgageData object to MortgageRequest.
    /// </summary>
    public static MortgageRequest ToMortgageRequest(this MortgageData mortgage)
    {
        var request = new MortgageRequest
        { 
            LoanType = LoanType.KBMortgage, // ProductTypeId
            PartnerId = mortgage.PartnerId.HasValue ? mortgage.PartnerId.Value : 0, //TODO: MortgageRequest.PartnerId should be nullable!
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
}
