using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOBY.Database.Entities;

[Table("FlowSwitch", Schema = "dbo")]
public sealed class FlowSwitch
{
    [Key]
    public int FlowSwitchId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool DefaultValue { get; set; }
}
