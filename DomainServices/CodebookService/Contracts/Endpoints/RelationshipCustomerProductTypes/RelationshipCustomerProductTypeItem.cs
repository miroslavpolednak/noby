using System.ComponentModel;
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

        [JsonIgnore]
        [DataMember(Order = 3)]
        [DefaultValue(MpHomeContractRelationshipType.NotSpecified)]
        public MpHomeContractRelationshipType MpHomeContractRelationshipType { get; set; }
    }
}
