using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedComponents.DocumentDataStorage.Database;

[Table("DocumentDataStorage", Schema = "dbo")]
internal sealed class DocumentDataStorage
    : CIS.Core.Data.BaseCreated
{
    [Key]
    public int DocumentDataStorageId { get; set; }
    public int DocumentDataEntityId { get; set; }
    public string DocumentDataType { get; set; } = null!;
    public int DocumentDataVersion { get; set; }
    public string? Data { get; set; }
}
