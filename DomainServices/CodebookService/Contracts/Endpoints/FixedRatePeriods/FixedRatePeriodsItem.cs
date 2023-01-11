namespace DomainServices.CodebookService.Contracts.Endpoints.FixedRatePeriods;

[DataContract]
public class FixedRatePeriodsItem
{
    [DataMember(Order = 1)]
    public int ProductTypeId { get; set; }

    [DataMember(Order = 2)]
    public int FixedRatePeriod { get; set; }

    [DataMember(Order = 3)]
    public int? MandantId { get; set; }

    [DataMember(Order = 4)]
    public bool IsNewProduct { get; set; }

    [DataMember(Order = 5)]
    public int InterestRateAlgorithm { get; set; }

    [DataMember(Order = 6)]
    public bool IsValid { get; set; }
}