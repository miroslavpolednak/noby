namespace DomainServices.CodebookService.Contracts.Endpoints.DeveloperSearch;

[DataContract]
public class DeveloperSearchRequest : IRequest<List<DeveloperSearchItem>>
{
    [DataMember(Order = 1)]
    public string Term { get; set; }
}
