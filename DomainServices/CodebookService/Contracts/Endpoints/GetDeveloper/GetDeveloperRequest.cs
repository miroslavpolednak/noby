namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloper;

[DataContract]
public class GetDeveloperRequest : IRequest<DeveloperItem>
{
    [DataMember(Order = 1)]
    public int DeveloperId { get; set; }
}
