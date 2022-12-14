﻿namespace DomainServices.CodebookService.Contracts.Endpoints.IncomeOtherTypes;

[DataContract]
public class IncomeOtherTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string Code { get; set; }

    [DataMember(Order = 4)]
    public bool IsValid { get; set; }

}