using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.RelationshipCustomerProductTypes
{
    [DataContract]
    public class RelationshipCustomerProductTypeItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        [JsonIgnore]
        public string RdmCode { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public string MpDigiApiCode { get; set; }

        [DataMember(Order = 5)]
        [JsonIgnore]
        public string NameNoby { get; set; }

    }
}
