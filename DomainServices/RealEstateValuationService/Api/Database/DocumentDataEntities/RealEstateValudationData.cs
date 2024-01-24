using System.Text.Json.Serialization;

namespace DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities;

/// <summary>
/// DocumentDataEntityId = RealEstateValuationId
/// </summary>
internal sealed class RealEstateValudationData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public SpecificDetailHouseAndFlatObject? HouseAndFlat { get; set; }
    public SpecificDetailParcelObject? Parcel { get; set; }
    public List<RealEstateValuationDocument>? Documents { get; set; }
    public List<int>? LoanPurposes { get; set; }
    public LocalSurveyData? LocalSurveyDetails { get; set; }
    public OnlinePreorderData? OnlinePreorderDetails { get; set; }

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

    public sealed class OnlinePreorderData
    {
        public string? BuildingTechnicalStateCode { get; set; }
        public string? BuildingMaterialStructureCode { get; set; }
        public string? FlatSchemaCode { get; set; }
        public decimal? FlatArea { get; set; }
        public string? BuildingAgeCode { get; set; }
    }

    public sealed class LocalSurveyData
    {
        public string? RealEstateValuationLocalSurveyFunctionCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneIDC { get; set; }
        public string? Email { get; set; }
    }
}
