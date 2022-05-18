﻿using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.PaymentDays
{
    [DataContract]
    public class PaymentDayItem
    {
        [DataMember(Order = 1)]
        public int PaymentDay { get; set; }

        [DataMember(Order = 2)]
        [JsonIgnore]
        public int PaymentAccountDay { get; set; }

        [DataMember(Order = 3)]
        [JsonIgnore]
        public string Mandant { get; set; }

        [DataMember(Order = 4)]
        public bool IsDefault { get; set; }
    }
}