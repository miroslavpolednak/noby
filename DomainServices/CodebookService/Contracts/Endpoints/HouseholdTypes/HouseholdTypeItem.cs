﻿using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.HouseholdTypes;

[DataContract]
public class HouseholdTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.HouseholdTypes EnumValue { get; set; }
    
    [DataMember(Order = 3)]
    public string Name { get; set; }

    [DataMember(Order = 4)]
    public string RdmCode { get; set; }

    [DataMember(Order = 5)]
    public int? DocumentTypeId { get; set; }
}