namespace DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities;

internal sealed class RealEstateValudationData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public SpecificDetailHouseAndFlatObject? HouseAndFlatDetails { get; set; }
    public SpecificDetailParcelObject? ParcelDetails { get; set; }
    public List<RealEstateValuationDocument>? Documents { get; set; }
    public List<int>? LoanPurposeDetails { get; set; }

    public sealed class SpecificDetailHouseAndFlatObject
    {
        public bool PoorCondition { get; set; }
        public bool OwnershipRestricted { get; set; }
        public SpecificDetailFlatOnlyDetails? FlatOnlyDetails { get; set; }
        public SpecificDetailFinishedHouseAndFlatDetails? FinishedHouseAndFlatDetails { get; set; }
    }

    public sealed class SpecificDetailFlatOnlyDetails
    {
        public bool SpecialPlacement { get; set; }
        public bool Basement { get; set; }
    }

    public sealed class SpecificDetailFinishedHouseAndFlatDetails
    {
        public bool Leased { get; set; }
        public bool LeaseApplicable { get; set; }
    }

    public sealed class SpecificDetailParcelObject
    {
        public List<SpecificDetailParcelNumber>? ParcelNumbers { get; set; }
    }

    public sealed class SpecificDetailParcelNumber
    {
        public int? Prefix { get; set; }
        public int? Number { get; set; }
    }

    public sealed class RealEstateValuationDocument
    {
        public string? DocumentInfoPrice { get; set; }
        public string? DocumentRecommendationForClient { get; set; }
    }
}
