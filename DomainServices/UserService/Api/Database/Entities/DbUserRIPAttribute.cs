using System.ComponentModel.DataAnnotations;

namespace DomainServices.UserService.Api.Database.Entities;

internal sealed class DbUserRIPAttribute
{
    [Key]
    public long PersonId { get; set; }
    public string? PersonSurname { get; set; }
    public long? PersonOrgUnitId { get; set; }
    public string? PersonOrgUnitName { get; set; }
    public string? PersonJobPostId { get; set; }
    public int? DealerCompanyId { get; set; }
}
