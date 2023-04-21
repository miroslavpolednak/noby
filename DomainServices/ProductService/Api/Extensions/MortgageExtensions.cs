using DomainServices.ProductService.Contracts;
using ExternalServices.MpHome.V1_1.Contracts;

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
            LoanType = LoanType.KBMortgage,
            ProductCodeUv = mortgage.ProductTypeId,
            PartnerId = mortgage.PartnerId,
            LoanContractNumber = mortgage.ContractNumber,
            LoanAmount = mortgage.LoanAmount,
            InterestRate = mortgage.LoanInterestRate,
            FixationPeriod = mortgage.FixedRatePeriod,
            MonthlyInstallment = mortgage.LoanPaymentAmount,
            LoanKind = mortgage.LoanKindId.GetValueOrDefault(),
            InstallmentDay = mortgage.PaymentDay,
            Expected1stDrawDate = mortgage.ExpectedDateOfDrawing,
            RepaymentAccountBank = mortgage.RepaymentAccount?.BankCode,
            RepaymentAccountNumber = mortgage.RepaymentAccount?.Number,
            RepaymentAccountPrefix = mortgage.RepaymentAccount?.Prefix,
            EstimatedDuePaymentDate = mortgage.LoanDueDate,
            RepaymentStartDate = mortgage.FirstAnnuityPaymentDate,
            ServiceBranchId = mortgage.BranchConsultantId,
            ConsultantId = mortgage.ThirdPartyConsultantId,
            LoanPurposes = mortgage.LoanPurposes is null ? null : mortgage.LoanPurposes.Select(t => new ExternalServices.MpHome.V1_1.Contracts.LoanPurpose
            {
                Amount = Convert.ToDouble((decimal)t.Sum),
                LoanPurposeId = t.LoanPurposeId
            }).ToList()
        };

        return request;
    }


    /// <summary>
    /// Converts db entity Loan (table dbo.Uver) to contract object MortgageData.
    /// </summary>
    public static MortgageData ToMortgage(this Database.Entities.Loan eLoan, List<Database.Entities.Relationship> eRelationships)
    {
        var mortgage = new MortgageData
        {
            PartnerId = (int)(eLoan.PartnerId ?? default),
            BranchConsultantId = eLoan.PobockaObsluhyId.HasValue ? Convert.ToInt32(eLoan.PobockaObsluhyId) : default,
            ThirdPartyConsultantId = eLoan.PoradceId.HasValue ? Convert.ToInt32(eLoan.PoradceId) : default,
            ContractNumber = eLoan.CisloSmlouvy,
            LoanAmount = eLoan.VyseUveru,
            LoanInterestRate = eLoan.RadnaSazba,
            FixedRatePeriod = eLoan.DelkaFixaceUrokoveSazby,
            ProductTypeId = eLoan.KodProduktyUv.GetValueOrDefault(),
            LoanPaymentAmount = eLoan.MesicniSplatka,
            CurrentAmount = eLoan.ZustatekCelkem,
            DrawingDateTo = eLoan.DatumKonceCerpani,
            ContractSignedDate = eLoan.DatumUzavreniSmlouvy,
            FixedRateValidTo = eLoan.DatumFixaceUrokoveSazby,
            AvailableForDrawing = eLoan.ZbyvaCerpat,
            Principal = eLoan.Jistina,
            LoanKindId = eLoan.DruhUveru,
            PaymentAccount = string.IsNullOrEmpty(eLoan.CisloUctu) ? null : new PaymentAccount
            {
                Prefix = eLoan.PredcisliUctu ?? "",
                Number = eLoan.CisloUctu ?? "",
                BankCode = "0100"//ma byt hardcoded
            },
            CurrentOverdueAmount = eLoan.CelkovyDluhPoSplatnosti,
            AllOverdueFees = eLoan.PohledavkaPoplatkyPo,
            OverdueDaysNumber = eLoan.PocetBankovnichDniPoSpl,
            ExpectedDateOfDrawing = eLoan.DatumPrvniVyplatyZUveru,
            InterestInArrears = eLoan.SazbaZProdleni,
            LoanDueDate = eLoan.DatumPredpSplatnosti,
            PaymentDay = eLoan.SplatkyDen,
            LoanInterestRateRefix = null,           // ???
            LoanInterestRateValidFromRefix = null,  // ???
            FixedRatePeriodRefix = null,            // ???
            FirstAnnuityPaymentDate = eLoan.PocatekSplaceni,
            RepaymentAccount = new PaymentAccount
            {
                BankCode = eLoan.InkasoBanka ?? "",
                Number = eLoan.InkasoCislo ?? "",
                Prefix = eLoan.InkasoPredcisli ?? ""
            },
            Statement = new StatementObject
            {
                TypeId = eLoan.HuVypisTyp,
                SubscriptionTypeId = eLoan.HuVypisZodb,
                FrequencyId = eLoan.HuVypisZodb,
                EmailAddress1 = eLoan.VypisEmail1,
                EmailAddress2 = eLoan.VypisEmail2
            }
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
    public static Relationship ToRelationship(this Database.Entities.Relationship eRelationship)
    {
        return new Relationship
        {
            PartnerId = (int)eRelationship.PartnerId,
            ContractRelationshipTypeId = eRelationship.VztahId
        };
    }
}

