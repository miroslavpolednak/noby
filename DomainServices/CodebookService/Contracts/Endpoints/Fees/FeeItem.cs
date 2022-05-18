using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Fees
{
    [DataContract]
    public class FeeItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        [JsonIgnore]
        public string IdKb { get; set; }

        [DataMember(Order = 3)]
        [JsonIgnore]
        public string Mandant { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public string ShortName { get; set; }

        [DataMember(Order = 5)]
        public string Name { get; set; }

        [DataMember(Order = 6)]
        [JsonIgnore]
        public bool IsValid { get; set; }
    }
}