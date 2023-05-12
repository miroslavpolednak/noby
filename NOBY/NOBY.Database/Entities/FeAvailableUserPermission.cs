using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOBY.Database.Entities;

[Table("FeAvailableUserPermission", Schema = "dbo")]
public sealed class FeAvailableUserPermission
{
    [Key]
    public int PermissionCode { get; set; }
}
