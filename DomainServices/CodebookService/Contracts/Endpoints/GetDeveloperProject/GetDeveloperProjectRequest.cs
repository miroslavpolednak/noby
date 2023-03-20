namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloperProject;

[DataContract]
public class GetDeveloperProjectRequest : IRequest<DeveloperProjectItem>
{
    [DataMember(Order = 1)]
    public int DeveloperProjectId { get; set; }
}
