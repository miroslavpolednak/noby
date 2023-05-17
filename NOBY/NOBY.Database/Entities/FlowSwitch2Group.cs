using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOBY.Database.Entities;

[Keyless]
[Table("FlowSwitch2Group", Schema = "dbo")]
public sealed class FlowSwitch2Group
{
    public int FlowSwitchGroupId { get; set; }

    public int FlowSwitchId { get; set; }

    public int GroupType { get; set; }

    public bool Value { get; set; }
}
