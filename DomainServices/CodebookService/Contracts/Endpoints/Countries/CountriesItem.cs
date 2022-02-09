using System.Runtime.Serialization;

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
        public string CurrencyCode { get; set; }

        [DataMember(Order = 5)]
        public bool IsAllowedForIncomeChange { get; set; }

        [DataMember(Order = 6)]
        public bool IsAllowedForResidenceChange { get; set; }
    }
}
