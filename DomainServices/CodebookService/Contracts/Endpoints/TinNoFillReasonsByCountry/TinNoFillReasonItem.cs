namespace DomainServices.CodebookService.Contracts.Endpoints.TinNoFillReasonsByCountry;

[DataContract]
public sealed class TinNoFillReasonItem
{
    [DataMember(Order = 1)]
    public string Id { get; set; }


    [DataMember(Order = 2)]
    public bool IsTinMandatory { get; set; }


    [DataMember(Order = 3)]
    public string ReasonForBlankTin { get; set; }


    [DataMember(Order = 4)]
    public bool IsValid { get; set; }
}