using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedComponents.DocumentDataStorage.Database;

[Table("DocumentDataStorage", Schema = "dbo")]
[PrimaryKey("DocumentDataEntityId", "DocumentDataType")]
internal sealed class DocumentDataStorage
    : CIS.Core.Data.BaseCreated
{
    public int DocumentDataEntityId { get; set; }
    public string DocumentDataType { get; set; } = null!;
    public int DocumentDataVersion { get; set; }
    public string? Data { get; set; }
}
