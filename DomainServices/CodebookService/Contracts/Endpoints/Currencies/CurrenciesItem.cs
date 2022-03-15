using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Currencies
{
    [DataContract]
    public class CurrenciesItem
    {
        [DataMember(Order = 1)]
        public string Code { get; set; }

        [DataMember(Order = 2)]
        [JsonIgnore]
        public bool AllowedForIncomeCurrency { get; set; }

        [DataMember(Order = 3)]
        [JsonIgnore]
        public bool AllowedForResidencyCurrency { get; set; }

        [DataMember(Order = 4)]
        public bool IsDefault { get; set; }
    }
}