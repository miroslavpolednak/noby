using System.ComponentModel.DataAnnotations;

namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloperProject;

[DataContract]
public class GetDeveloperProjectRequest : IRequest<DeveloperProjectItem>
{
    /// <summary>
    /// Id developera z číselníku developerských projektů
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public int DeveloperId { get; set; }

    /// <summary>
    /// Id developerského projektu z číselníku developerských projektů
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public int DeveloperProjectId { get; set; }
}
