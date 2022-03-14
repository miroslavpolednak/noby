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
        [JsonIgnore]
        public string ShortName { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public string LongName { get; set; }

        [DataMember(Order = 5)]
        public bool IsDefault { get; set; }

        [DataMember(Order = 6)]
        [JsonIgnore]
        public bool Risk { get; set; }

        [DataMember(Order = 7)]
        [JsonIgnore]
        public bool EuMember { get; set; }

        [DataMember(Order = 8)]
        [JsonIgnore]
        public bool Eurozone { get; set; }
    }
}