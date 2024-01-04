using SharedComponents.DocumentDataStorage;

namespace DomainServices.SalesArrangementService.Api.Database.DocumentDataEntities;

internal sealed class MortgageData : IDocumentData
{
    int IDocumentData.Version => 1;

    public DateTime? ExpectedDateOfDrawing { get; set; }

    public string IncomeCurrencyCode { get; set; } = null!;

    public string ResidencyCurrencyCode { get; set; } = null!;

    public int? ContractSignatureTypeId { get; set; }

    public ICollection<LoanRealEstateData> LoanRealEstates { get; set; } = new List<LoanRealEstateData>();

    public int? Agent { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime? FirstSignatureDate { get; set; }

    public class LoanRealEstateData
    {
        public int RealEstateTypeId { get; set; }

        public bool IsCollateral { get; set; }

        public int RealEstatePurchaseTypeId { get; set; }
    }
}