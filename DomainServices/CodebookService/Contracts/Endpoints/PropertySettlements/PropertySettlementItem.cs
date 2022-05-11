using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.PropertySettlements
{
    [DataContract]
    public class PropertySettlementItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        [JsonIgnore]
        public string Code { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public string NameEng { get; set; }

        [DataMember(Order = 5)]
        [JsonIgnore]
        public bool IsValid { get; set; }
    }
}