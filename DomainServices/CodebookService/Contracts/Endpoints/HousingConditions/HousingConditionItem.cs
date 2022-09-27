namespace DomainServices.CodebookService.Contracts.Endpoints.HousingConditions
{
    [DataContract]
    public class HousingConditionItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }
        
        [DataMember(Order = 3)]
        public string Code { get; set; }

        [DataMember(Order = 4)]
        public string RdmCode { get; set; }

        [DataMember(Order = 5)]
        public bool IsValid { get; set; }
    }
}