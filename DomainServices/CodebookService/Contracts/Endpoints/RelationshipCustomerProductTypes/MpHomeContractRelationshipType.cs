namespace DomainServices.CodebookService.Contracts.Endpoints.RelationshipCustomerProductTypes
{
    [DataContract]
    public enum MpHomeContractRelationshipType : byte
    {
        [DataMember(Order = 1)]
        [EnumMember]
        NotSpecified = 0,

        [DataMember(Order = 2)]
        [EnumMember]
        Owner = 1,

        [DataMember(Order = 3)]
        [EnumMember]
        CoDebtor = 2,

        [DataMember(Order = 4)]
        [EnumMember]
        Accessor = 3,

        [DataMember(Order = 5)]
        [EnumMember]
        HusbandOrWife = 4,

        [DataMember(Order = 6)]
        [EnumMember]
        LegalRepresentative = 5,

        [DataMember(Order = 7)]
        [EnumMember]
        CollisionGuardian = 6,

        [DataMember(Order = 8)]
        [EnumMember]
        Guardian = 7,

        [DataMember(Order = 9)]
        [EnumMember]
        Guarantor = 8,

        [DataMember(Order = 10)]
        [EnumMember]
        GuarantorHusbandOrWife = 9,

        [DataMember(Order = 11)]
        [EnumMember]
        Intermediary = 11,

        [DataMember(Order = 12)]
        [EnumMember]
        ManagingDirector = 12,

        [DataMember(Order = 13)]
        [EnumMember]
        Child = 13,

    }
}