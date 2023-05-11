using System.ComponentModel.DataAnnotations;

namespace DomainServices.UserService.Api.Database.Entities;

internal sealed class DbUserIdentity
{
    [Key]
    public int v33id { get; set; }
    public string? firstname { get; set; }
    public string? surname { get; set; }
    public string? kbuid { get; set;}
    public long? brokerId { get; set; }
    public int? m04id { get; set; }
    public int? m17id { get; set; }
    public int? oscis { get; set; }
    public string? cpm { get; set; }
    public string? icp { get; set; }
    public string? ic { get; set; }
    public string? kbad { get; set; }
    public string? mpad { get; set; }
}
