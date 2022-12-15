namespace DomainServices.CodebookService.Contracts.Endpoints.CountryCodePhoneIdc;

[DataContract]
public class CountryCodePhoneIdcItem
{
    [DataMember(Order = 1)]
    public string Id { get; set; }


    [DataMember(Order = 2)]
    public string Name { get; set; }


    [DataMember(Order = 3)]
    public string Idc { get; set; }


    [DataMember(Order = 4)]
    public bool IsValid { get; set; }
}
