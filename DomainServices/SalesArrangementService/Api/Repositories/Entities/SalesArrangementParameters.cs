using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("SalesArrangementParameters", Schema = "dbo")]
internal class SalesArrangementParameters
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesArrangementParametersId { get; set; }

    public int SalesArrangementId { get; set; }

    public SalesArrangementParametersTypes SalesArrangementParametersType { get; set; }

    public string? Parameters { get; set; }
    
    public byte[]? ParametersBin { get; set; }
}

internal enum SalesArrangementParametersTypes : byte
{
    Mortgage = 1,
    Drawing = 2,
    GeneralChange = 3,
    HUBN = 4
}
