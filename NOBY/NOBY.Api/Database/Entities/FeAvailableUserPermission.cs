using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOBY.Api.Database.Entities;

[Table("FeAvailableUserPermission", Schema = "dbo")]
internal sealed class FeAvailableUserPermission
{
    [Key]
    public int PermissionCode { get; set; }
}
