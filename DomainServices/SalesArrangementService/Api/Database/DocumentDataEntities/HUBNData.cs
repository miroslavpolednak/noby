using SharedComponents.DocumentDataStorage;
using SharedTypes.Types;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class HUBNData : IDocumentData
{
    int IDocumentData.Version => 1;

    public IEnumerable<CustomerIdentity> Applicant { get; set; } = Enumerable.Empty<CustomerIdentity>();
    public HUBNLoanAmountData? LoanAmount { get; set; }
    public IEnumerable<HUBNLoanPurposeItemData> LoanPurposes { get; set; } = Enumerable.Empty<HUBNLoanPurposeItemData>();
    public IEnumerable<HUBNLoanRealEstateItemData> LoanRealEstates { get; set; } = Enumerable.Empty<HUBNLoanRealEstateItemData>();
    public HUBNCollateralIdentificationData? CollateralIdentification { get; set; }
    public HUBNExpectedDateOfDrawingData? ExpectedDateOfDrawing { get; set; }
    public HUBNDrawingDateToData? DrawingDateTo { get; set; }
    public HUBNCommentToChangeRequestData? CommentToChangeRequest { get; set; }

    public class HUBNLoanAmountData
    {
        public bool ChangeAgreedLoanAmount { get; set; }
        public decimal AgreedLoanAmount { get; set; }
        public decimal? RequiredLoanAmount { get; set; }
        public bool PreserveAgreedLoanDueDate { get; set; }
        public DateTime AgreedLoanDueDate { get; set; }
        public bool PreserveAgreedLoanPaymentAmount { get; set; }
        public decimal AgreedLoanPaymentAmount { get; set; }
    }

    public class HUBNLoanPurposeItemData
    {
        public int LoanPurposeId { get; set; }
        public decimal Sum { get; set; }
    }

    public class HUBNLoanRealEstateItemData
    {
        public int RealEstateTypeId { get; set; }
        public int RealEstatePurchaseTypeId { get; set; }
        public bool IsCollateral { get; set; }
    }

    public class HUBNCollateralIdentificationData
    {
        public string? RealEstateIdentification { get; set; } = null!;
    }

    public class HUBNExpectedDateOfDrawingData
    {
        public bool IsActive { get; set; }
        public DateTime? NewExpectedDateOfDrawing { get; set; }
        public DateTime AgreedExpectedDateOfDrawing { get; set; }
    }

    public class HUBNDrawingDateToData
    {
        public bool IsActive { get; set; }
        public DateTime AgreedDrawingDateTo { get; set; }
        public int? ExtensionDrawingDateToByMonths { get; set; }
    }

    public class HUBNCommentToChangeRequestData
    {
        public bool IsActive { get; set; }
        public string? GeneralComment { get; set; }
    }
}