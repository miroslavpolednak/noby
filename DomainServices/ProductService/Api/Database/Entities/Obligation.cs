using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("Zavazky", Schema = "dbo")]
internal class Obligation
{
    [Key, Column("UverId")]
    public long LoanId { get; set; }

    [Key, Column("UcelUveruInt")]
    public int LoanPurposeId { get; set; }

    [Column("TypZavazku")]
    public int ObligationTypeId { get; set; }

    [Column("Castka")]
    public decimal Amount { get; set; }

    [Column("Veritel")]
    public string? CreditorName { get; set; }

    [Column("PredcisliUctu")]
    public string? AccountNumberPrefix { get; set; }

    [Column("CisloUctu")]
    public string? AccountNumber { get; set; }

    [Column("KodBanky")]
    public string? BankCode { get; set; }

    [Column("VariabilniSymbol")]
    public string?  VariableSymbol { get; set; }
}