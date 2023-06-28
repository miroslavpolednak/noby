using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOBY.Database.Entities;

[Table("FlowSwitchGroup", Schema = "dbo")]
public sealed class FlowSwitchGroup
{
    [Key]
    public int FlowSwitchGroupId { get; set; }

    public string? Description { get; set; }

    public bool IsVisibleDefault { get; set; }

    public bool IsActiveDefault { get; set; }

    public bool IsCompletedDefault { get; set; }
}
