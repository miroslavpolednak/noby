namespace DomainServices.CodebookService.Contracts.Endpoints.MaritalStatuses
{
    // Codebook used by RIP (avoid use attribute 'JsonIgnore')

    [DataContract]
    public class MaritalStatusItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public string RdmMaritalStatusCode { get; set; }
        
        [DataMember(Order = 4)]
        public bool IsDefault { get; set; }
    }
}