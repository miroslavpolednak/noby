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
            LoanKind = mortgage.LoanKindId.GetValueOrDefault()
            //TODO: add mapping (not specified so far)
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
            PartnerId = (int)(eLoan.PartnerId ?? default),
            ContractNumber = eLoan.CisloSmlouvy,
            LoanAmount = eLoan.VyseUveru,
            LoanInterestRate = eLoan.RadnaSazba,
            FixedRatePeriod = eLoan.DelkaFixaceUrokoveSazby,
            ProductTypeId = eLoan.TypUveru,
            LoanPaymentAmount = eLoan.MesicniSplatka,
            CurrentAmount = eLoan.ZustatekCelkem,
            DrawingDateTo = eLoan.DatumKonceCerpani,
            ContractSignedDate = eLoan.DatumUzavreniSmlouvy,
            FixedRateValidTo = eLoan.DatumFixaceUrokoveSazby,
            AvailableForDrawing = eLoan.ZbyvaCerpat,
            // Principal = eLoan.Jistina,           // ???
            LoanKindId = eLoan.DruhUveru,
            PaymentAccount = string.IsNullOrEmpty(eLoan.CisloUctu) ? null : new PaymentAccount
            {
                Prefix = eLoan.PredcisliUctu ?? "",
                Number = eLoan.CisloUctu ?? "",
                BankCode = "0100"//ma byt hardcoded
            },
            CurrentOverdueAmount = null,            // ???
            AllOverdueFees = null,                  // ???
            OverdueDaysNumber = null,               // ???
            // LoanPurposes = null,                 // ???
            ExpectedDateOfDrawing = eLoan.DatumPrvniVyplatyZUveru,
            InterestInArrears = null,               // ???
            LoanDueDate = null,                     // ???
            PaymentDay = null,                      // ???
            LoanInterestRateRefix = null,           // ???
            LoanInterestRateValidFromRefix = null,  // ???
            FixedRatePeriodRefix = null,            // ???
            // Cpm = eLoan.CPM,                     // ???
            // Icp = eLoan.PoradceId,               // ???
            FirstAnnuityPaymentDate = null,         // ???
            RepaymentAccount = null                 // ???
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

