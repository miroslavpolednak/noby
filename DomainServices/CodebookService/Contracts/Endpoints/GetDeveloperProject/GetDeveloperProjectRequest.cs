namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloperProject;

[DataContract]
public class GetDeveloperProjectRequest : IRequest<DeveloperProjectItem>
{
    public int DeveloperProjectId { get; set; }
}
