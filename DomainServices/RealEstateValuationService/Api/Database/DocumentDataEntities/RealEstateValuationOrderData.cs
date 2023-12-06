using SharedTypes.Enums;

namespace DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities;

/// <summary>
/// DocumentDataEntityId = RealEstateValuationId
/// </summary>
internal sealed class RealEstateValuationOrderData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public RealEstateValuationOrderTypes RealEstateValuationOrderType { get; set; }
    public StandardOrderData? Standard { get; set; }
    public OnlinePreorderData? OnlinePreorder { get; set; }

    public sealed class OnlinePreorderData
    {
        public string? BuildingTechnicalStateCode { get; set; }
        public string? BuildingMaterialStructureCode { get; set; }
        public string? FlatSchemaCode { get; set; }
        public decimal? FlatArea { get; set; }
        public string? BuildingAgeCode { get; set; }
    }

    public sealed class StandardOrderData
    {
        public string? RealEstateValuationLocalSurveyFunctionCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneIDC { get; set; }
        public string? Email { get; set; }
    }
}
