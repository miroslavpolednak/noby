﻿namespace DomainServices.CodebookService.Contracts.Endpoints.PropertySettlements
{
    [DataContract]
    public class PropertySettlementItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        public string NameEng { get; set; }

        [DataMember(Order = 5)]
        public bool IsValid { get; set; }
    }
}