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
        public int MandantId { get; set; }

        [DataMember(Order = 4)]
        public CIS.Foms.Enums.Mandants Mandant { get; set; }

        [DataMember(Order = 5)]
        public string ProductTypeId { get; set; }

        [DataMember(Order = 6)]
        public int Order { get; set; }

        [DataMember(Order = 7)]
        [JsonIgnore]
        public bool IsValid { get; set; }
    }
}
