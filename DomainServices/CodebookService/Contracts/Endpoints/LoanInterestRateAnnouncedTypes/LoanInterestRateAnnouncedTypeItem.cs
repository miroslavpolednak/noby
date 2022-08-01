﻿namespace DomainServices.CodebookService.Contracts.Endpoints.LoanInterestRateAnnouncedTypes
{
    [DataContract]
    public class LoanInterestRateAnnouncedTypeItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Code { get; set; }

        [DataMember(Order = 3)]
        public string Name { get; set; }

    }
}