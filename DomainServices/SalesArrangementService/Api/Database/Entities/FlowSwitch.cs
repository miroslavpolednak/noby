using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Database.Entities;

[Table("FlowSwitch", Schema = "dbo")]
[PrimaryKey("SalesArrangementId", "FlowSwitchId")]
internal sealed class FlowSwitch
{
    public int SalesArrangementId { get; set; }

    public int FlowSwitchId { get; set; }

    public bool Value { get; set; }
}
