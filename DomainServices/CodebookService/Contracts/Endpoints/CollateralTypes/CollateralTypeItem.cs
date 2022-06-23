namespace DomainServices.CodebookService.Contracts.Endpoints.CollateralTypes
{
    [DataContract]
    public class CollateralTypeItem
    {
        [DataMember(Order = 1)]
        public int CollateralType { get; set; }

        [DataMember(Order = 2)]
        public int MandantId { get; set; }

        [DataMember(Order = 3)]
        public string CodeBgm { get; set; }

        [DataMember(Order = 4)]
        public string TextBgm { get; set; }

        [DataMember(Order = 5)]
        public string NameType { get; set; }
    }
}