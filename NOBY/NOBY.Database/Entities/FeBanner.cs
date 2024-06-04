using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOBY.Database.Entities;

[Table("FeBanner", Schema = "dbo")]
public sealed class FeBanner
    : CIS.Core.Data.BaseCreated
{
    [Key]
    public int FeBannerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int Severity { get; set; }
    public DateTime VisibleFrom { get; set; }
    public DateTime VisibleTo { get; set; }
}
