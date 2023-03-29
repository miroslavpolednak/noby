using System.ComponentModel.DataAnnotations;

namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloper;

[DataContract]
public class GetDeveloperRequest : IRequest<DeveloperItem>
{
    /// <summary>
    /// Id developera z číselníku developerských projektů
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public int DeveloperId { get; set; }
}
