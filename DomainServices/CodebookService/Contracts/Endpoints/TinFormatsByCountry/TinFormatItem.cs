namespace DomainServices.CodebookService.Contracts.Endpoints.TinFormatsByCountry;

[DataContract]
public sealed class TinFormatItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public string CountryCode { get; set; }


    [DataMember(Order = 3)]
    public string RegularExpression { get; set; }


    [DataMember(Order = 4)]
    public bool IsForFo { get; set; }


    [DataMember(Order = 5)]
    public string Tooltip { get; set; }


    [DataMember(Order = 6)]
    public bool IsValid { get; set; }
}