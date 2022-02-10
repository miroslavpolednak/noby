namespace DomainServices.CodebookService.Contracts.Endpoints.ProductTypes
{
    [DataContract]
    public enum ProductTypeCategory : byte
    {
        [DataMember(Order = 1)]
        [EnumMember]
        Unknown = 0,

        [DataMember(Order = 2)]
        [EnumMember]
        BuildingSavings = 1,

        [DataMember(Order = 3)]
        [EnumMember]
        BuildingSavingsLoan = 2,

        [DataMember(Order = 4)]
        [EnumMember]
        Mortgage = 3
    }
}
