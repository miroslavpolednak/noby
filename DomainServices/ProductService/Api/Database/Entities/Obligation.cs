using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("Zavazky", Schema = "dbo")]
internal class Obligation
{
    [Column("UverId")]
    public long LoanId { get; set; }

    [Column("UcelUveruInt")]
    public short LoanPurposeId { get; set; }

    [Column("TypZavazku")]
    public short ObligationTypeId { get; set; }

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