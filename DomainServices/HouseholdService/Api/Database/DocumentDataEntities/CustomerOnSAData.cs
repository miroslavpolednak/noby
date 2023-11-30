namespace DomainServices.HouseholdService.Api.Database.DocumentDataEntities;

internal sealed class CustomerOnSAData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public CustomerAdditionalData? AdditionalData { get; set; }
    public CustomerChangeMetadata? ChangeMetadata { get; set; }

    internal sealed class CustomerChangeMetadata
    {
        public bool WereClientDataChanged { get; set; }
        public bool WasCRSChanged { get; set; }
    }

    internal sealed class CustomerAdditionalData
    {
        public bool IsAddressWhispererUsed { get; set; }
        public bool HasRelationshipWithKB { get; set; }
        public bool HasRelationshipWithKBEmployee { get; set; }
        public bool HasRelationshipWithCorporate { get; set; }
        public bool IsPoliticallyExposed { get; set; }
        public bool IsUSPerson { get; set; }
        public LegalCapacityData? LegalCapacity { get; set; }
    }

    internal sealed class LegalCapacityData
    {
        public int? RestrictionTypeId { get; set; }
        public DateTime? RestrictionUntil { get; set; }
    }
}
