namespace DomainServices.CodebookService.Contracts.Endpoints.Countries
{
    [DataContract]
    public class CountriesItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string ShortName { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        public string LongName { get; set; }

        [DataMember(Order = 5)]
        public bool IsDefault { get; set; }

        [DataMember(Order = 6)]
        public bool Risk { get; set; }

        [DataMember(Order = 7)]
        public bool EuMember { get; set; }

        [DataMember(Order = 8)]
        public bool Eurozone { get; set; }
    }
}