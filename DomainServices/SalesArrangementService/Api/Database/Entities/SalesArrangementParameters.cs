using CIS.Foms.Types.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Database.Entities;

[Table("SalesArrangementParameters", Schema = "dbo")]
internal sealed class SalesArrangementParameters
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesArrangementParametersId { get; set; }

    public int SalesArrangementId { get; set; }

    public SalesArrangementTypes SalesArrangementParametersType { get; set; }

    public string? Parameters { get; set; }
    
    public byte[]? ParametersBin { get; set; }
}
