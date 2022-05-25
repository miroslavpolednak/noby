namespace DomainServices.CodebookService.Contracts.Endpoints.MarketingActions
{
    // Codebook used by RIP (avoid use attribute 'JsonIgnore')

    [DataContract]
    public class MarketingActionItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; }

        [DataMember(Order = 3)]
        public int MandantId { get; set; }

        [DataMember(Order = 4)]
        public string Name { get; set; }

        [DataMember(Order = 5)]
        public string Description { get; set; }
        
        [DataMember(Order = 6)]
        public bool IsValid { get; set; }
    }
}