using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOBY.Database.Entities;

[Table("FeAvailableUserPermission", Schema = "dbo")]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public sealed class FeAvailableUserPermission
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    [Key]
    public int PermissionCode { get; set; }
}
