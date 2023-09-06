using System.ComponentModel.DataAnnotations;

namespace DomainServices.UserService.Api.Database.Entities;

internal sealed class DbUserAttribute
{
    [Key]
    public int v33id { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
    public string? VIPFlag { get; set; }
    public int? distributionChannelId { get; set; }
    public string? personOrgUnitName { get; set; }
    public string? dealerCompanyName { get; set; }
}
