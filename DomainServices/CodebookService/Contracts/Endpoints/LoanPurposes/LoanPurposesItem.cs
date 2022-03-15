using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes
{
    [DataContract]
    public sealed class LoanPurposesItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public CIS.Foms.Enums.Mandants Mandant { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public bool IsValid { get; set; }
    }
}
