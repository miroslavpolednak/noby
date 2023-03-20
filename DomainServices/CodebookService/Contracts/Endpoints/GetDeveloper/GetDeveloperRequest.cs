namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloper;

[DataContract]
public class GetDeveloperRequest : IRequest<DeveloperItem>
{
    public int DeveloperId { get; set; }
}
