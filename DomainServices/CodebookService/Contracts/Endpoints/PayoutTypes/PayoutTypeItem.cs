﻿using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.PayoutTypes;

[DataContract]
public class PayoutTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.PayoutTypes EnumValue { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }
}
