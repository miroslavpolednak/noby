using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.UserService.Api.Database.Entities;

[Keyless]
internal sealed class DbUserRIPAttribute
{
    public long? PersonId { get; set; }
    public string? PersonSurname { get; set; }
    public long? PersonOrgUnitId { get; set; }
    public string? PersonOrgUnitName { get; set; }
    public string? PersonJobPostId { get; set; }
    public int? DealerCompanyId { get; set; }

    //az bude pripraveno od Bobruska/Kriz https://wiki.kb.cz/pages/viewpage.action?pageId=527429927
    [NotMapped]
    public string? Company { get; set; }
    [NotMapped]
    public long? BrokerId { get; set; }
}
