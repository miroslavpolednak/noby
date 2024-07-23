using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Database.Entities;

[Table("CaseIdAccountNumberKonstDb", Schema = "dbo")]
[Keyless]
internal sealed class CaseIdAccountNumberKonstDb
{
    public long CaseId { get; set; }

    public string AreaCodeAccountNumber { get; set; } = null!;
}
