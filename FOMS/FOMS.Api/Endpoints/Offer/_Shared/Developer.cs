namespace FOMS.Api.Endpoints.Offer.Dto;

public sealed class Developer
{
    public int? DeveloperId { get; set; }
    public int? ProjectId { get; set; }
    public string? NewDeveloperName { get; set; }
    public string? NewDeveloperProjectName { get; set; }
    public string? NewDeveloperCin { get; set; }
}
