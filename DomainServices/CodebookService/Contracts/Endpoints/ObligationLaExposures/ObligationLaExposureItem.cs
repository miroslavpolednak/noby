namespace DomainServices.CodebookService.Contracts.Endpoints.ObligationLaExposures;

[DataContract]
public class ObligationLaExposureItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string RdmCode { get; set; }

    [DataMember(Order = 4)]
    public int ObligationTypeId { get; set; }

    [DataMember(Order = 5)]
    public bool IsValid { get; set; }

}
