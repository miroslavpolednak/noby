using SharedComponents.DocumentDataStorage;
using SharedTypes.Types;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class GeneralChangeData : IDocumentData
{
    int IDocumentData.Version => 1;

    public IEnumerable<CustomerIdentity> Applicant { get; set; } = Enumerable.Empty<CustomerIdentity>();
    public GeneralChangeCollateralData? Collateral { get; set; }
    public GeneralChangePaymentDayData? PaymentDay { get; set; }
    public GeneralChangeDrawingDateToData? DrawingDateTo { get; set; }
    public GeneralChangePaymentAccountData? RepaymentAccount { get; set; }
    public GeneralChangeLoanPaymentAmountData? LoanPaymentAmount { get; set; }
    public GeneralChangeDueDateData? DueDate { get; set; }
    public GeneralChangeLoanRealEstateData? LoanRealEstate { get; set; }
    public GeneralChangeLoanPurposeData? LoanPurpose { get; set; }
    public GeneralChangeDrawingAndOtherConditionsData? DrawingAndOtherConditions { get; set; }
    public GeneralChangeCommentToChangeRequestData? CommentToChangeRequest { get; set; }

    public sealed class GeneralChangeCollateralData
    {
        public bool IsActive { get; set; }
        public string? AddLoanRealEstateCollateral { get; set; }
        public string? ReleaseLoanRealEstateCollateral { get; set; }
    }

    public sealed class GeneralChangePaymentDayData
    {
        public bool IsActive { get; set; }
        public int AgreedPaymentDay { get; set; }
        public int? NewPaymentDay { get; set; }
    }

    public sealed class GeneralChangeDrawingDateToData
    {
        public bool IsActive { get; set; }
        public DateTime AgreedDrawingDateTo { get; set; }
        public int? ExtensionDrawingDateToByMonths { get; set; }
        public string? CommentToDrawingDateTo { get; set; }
    }

    public sealed class GeneralChangePaymentAccountData
    {
        public bool IsActive { get; set; }
        public string AgreedPrefix { get; set; } = null!;
        public string AgreedNumber { get; set; } = null!;
        public string AgreedBankCode { get; set; } = null!;
        public string? Prefix { get; set; }
        public string? Number { get; set; }
        public string? BankCode { get; set; }
        public string? OwnerFirstName { get; set; }
        public string? OwnerLastName { get; set; }
        public DateTime? OwnerDateOfBirth { get; set; }
    }

    public sealed class GeneralChangeLoanPaymentAmountData
    {
        public bool IsActive { get; set; }
        public decimal? NewLoanPaymentAmount { get; set; }
        public decimal ActualLoanPaymentAmount { get; set; }
        public bool ConnectionExtraordinaryPayment { get; set; }
    }

    public sealed class GeneralChangeDueDateData
    {
        public bool IsActive { get; set; }
        public DateTime? NewLoanDueDate { get; set; }
        public DateTime ActualLoanDueDate { get; set; }
        public bool ConnectionExtraordinaryPayment { get; set; }
    }

    public sealed class GeneralChangeLoanRealEstateData
    {
        public bool IsActive { get; set; }
        public IEnumerable<GeneralChangeLoanRealEstatesItemData> LoanRealEstates { get; set; } = Enumerable.Empty<GeneralChangeLoanRealEstatesItemData>();
    }

    public sealed class GeneralChangeLoanRealEstatesItemData
    {
        public int RealEstateTypeId { get; set; }
        public int RealEstatePurchaseTypeId { get; set; }
    }

    public sealed class GeneralChangeLoanPurposeData
    {
        public bool IsActive { get; set; }
        public string? LoanPurposesComment { get; set; }
    }

    public sealed class GeneralChangeDrawingAndOtherConditionsData
    {
        public bool IsActive { get; set; }
        public string? CommentToChangeContractConditions { get; set; }
    }

    public sealed class GeneralChangeCommentToChangeRequestData
    {
        public bool IsActive { get; set; }
        public string? GeneralComment { get; set; }
    }


}