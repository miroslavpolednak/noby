using DomainServices.ProductService.Contracts;
using ExternalServices.MpHome.V1.Contracts;
using LoanPurpose = DomainServices.ProductService.Contracts.LoanPurpose;

namespace DomainServices.ProductService.Api;

internal static class MortgageExtensions
{
    /// <summary>
    /// Converts contract object MortgageData to MortgageRequest.
    /// </summary>
    public static MortgageRequest ToMortgageRequest(this MortgageData mortgage, string? pcpId)
    {
        var request = new MortgageRequest
        {
            PcpInstId = pcpId,
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
            ConsultantId = mortgage.CaseOwnerUserCurrentId,
            FirstRequestSignDate = mortgage.FirstSignatureDate,
            LoanPurposes = mortgage.LoanPurposes?.Select(t => new global::ExternalServices.MpHome.V1.Contracts.LoanPurpose
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
    public static MortgageData ToMortgage(this Database.Models.Loan loan, List<Database.Models.Relationship> relationships)
    {
        var mortgage = new MortgageData
        {
            PartnerId = (int)(loan.PartnerId ?? default),
            BranchConsultantId = loan.BranchConsultantId,
            CaseOwnerUserCurrentId = loan.CaseOwnerUserCurrentId,
            CaseOwnerUserOrigId = loan.CaseOwnerUserOrigId,
            ContractNumber = loan.ContractNumber,
            LoanAmount = loan.LoanAmount,
            LoanInterestRate = loan.LoanInterestRate,
            FixedRatePeriod = loan.FixedRatePeriod,
            ProductTypeId = loan.ProductTypeId.GetValueOrDefault(),
            LoanPaymentAmount = loan.LoanPaymentAmount,
            CurrentAmount = loan.CurrentAmount,
            DrawingDateTo = loan.DrawingDateTo,
            ContractSignedDate = loan.ContractSignedDate,
            FixedRateValidTo = loan.FixedRateValidTo,
            AvailableForDrawing = loan.AvailableForDrawing,
            Principal = loan.Principal,
            LoanKindId = loan.LoanKindId,
            PaymentAccount = ParsePaymentAccount(loan),
            CurrentOverdueAmount = loan.CurrentOverdueAmount,
            AllOverdueFees = loan.AllOverdueFees,
            OverdueDaysNumber = loan.OverdueDaysNumber,
            ExpectedDateOfDrawing = loan.ExpectedDateOfDrawing,
            InterestInArrears = loan.InterestInArrears,
            LoanDueDate = loan.LoanDueDate,
            PaymentDay = loan.PaymentDay,
            LoanInterestRateRefix = null, // ???
            LoanInterestRateValidFromRefix = null, // ???
            FixedRatePeriodRefix = null, // ???
            FirstAnnuityPaymentDate = loan.FirstAnnuityPaymentDate,
            FirstSignatureDate = loan.FirstSignatureDate,
            RepaymentAccount = ParseRepaymentAccount(loan),
            Statement = ParseStatementObject(loan)
        };

        if (relationships.Any())
        {
            mortgage.Relationships.AddRange(relationships.Select(ToRelationship));
        }

        return mortgage;
    }

    private static PaymentAccount? ParsePaymentAccount(this Database.Models.Loan loan) =>
        string.IsNullOrEmpty(loan.PaymentAccountNumber)
            ? null
            : new PaymentAccount
            {
                Prefix = loan.PaymentAccountPrefix ?? string.Empty,
                Number = loan.PaymentAccountNumber ?? string.Empty,
                BankCode = "0100" //ma byt hardcoded
            };

    private static PaymentAccount ParseRepaymentAccount(this Database.Models.Loan loan) => new()
    {
        BankCode = loan.RepaymentAccountBankCode ?? string.Empty,
        Number = loan.RepaymentAccountNumber ?? string.Empty,
        Prefix = loan.RepaymentAccountPrefix ?? string.Empty
    };
    
    private static StatementObject ParseStatementObject(this Database.Models.Loan loan) => new()
    {
        TypeId = loan.StatementTypeId,
        SubscriptionTypeId = loan.StatementSubscriptionTypeId,
        FrequencyId = loan.StatementFrequencyId,
        EmailAddress1 = loan.EmailAddress1,
        EmailAddress2 = loan.EmailAddress2
    };
    
    /// <summary>
    /// Maps db entity LoanPurpose (table dbo.UverUcely) to contract object LoanPurpose .
    /// </summary>
    public static LoanPurpose ToLoanPurpose(this Database.Models.LoanPurpose loanPurpose) => new()
    {
        LoanPurposeId = loanPurpose.LoanPurposeId,
        Sum = loanPurpose.Sum
    };
    
    /// <summary>
    /// Maps db entity Relationship (table dbo.VztahUver) to contract object Relationship .
    /// </summary>
    public static Relationship ToRelationship(this Database.Models.Relationship relationship) => new()
    {
        PartnerId = relationship.PartnerId,
        ContractRelationshipTypeId = relationship.ContractRelationshipTypeId
    };
}