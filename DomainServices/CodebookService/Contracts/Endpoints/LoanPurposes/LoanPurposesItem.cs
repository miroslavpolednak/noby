using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes
{
    [DataContract]
    public class LoanPurposesItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public int MandantId { get; set; }

        [DataMember(Order = 4)]
        public List<int> ProductTypeIds { get; set; }

        [DataMember(Order = 5)]
        public int Order { get; set; }

        [DataMember(Order = 6)]
        [JsonIgnore]
        public bool IsValid { get; set; }
    }
}