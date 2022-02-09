﻿using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates
{
    [DataContract]
    public class SalesArrangementStateItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        
        [DataMember(Order = 2)]
        public CIS.Core.Enums.SalesArrangementStates Value { get; set; }
        
        [DataMember(Order = 3)]
        public string Name { get; set; }

        [DataMember(Order = 4)]
        [JsonIgnore]
        public bool IsDefault { get; set; }
    }
}
