using CIS.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOBY.Database.Entities;

[Table("TempFile", Schema = "dbo")]
public sealed class TempFile
    : BaseCreated
{
    [Key]
    public Guid TempFileId { get; set; }

    public string FileName { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public long? ObjectId { get; set; }
    public string? ObjectType { get; set; }
    public Guid? SessionId { get; set; }
}
