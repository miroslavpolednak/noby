using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Countries
{
    [DataContract]
    public class CountriesItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string FullName { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public string CurrencyCode { get; set; }

        [DataMember(Order = 5)]
        [JsonIgnore]
        public bool IsAllowedForIncomeChange { get; set; }

        [DataMember(Order = 6)]
        [JsonIgnore]
        public bool IsAllowedForResidenceChange { get; set; }
    }
}
