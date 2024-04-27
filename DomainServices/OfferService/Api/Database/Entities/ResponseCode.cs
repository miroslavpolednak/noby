using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Database.Entities;

[Table("ResponseCode", Schema = "dbo")]
internal sealed class ResponseCode
    : CIS.Core.Data.BaseCreated
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ResponseCodeId { get; set; }

    public long CaseId { get; set; }

    public int? ResponseCodeTypeId { get; set; }

    public string? Data { get; set; }
}
