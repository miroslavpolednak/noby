using DomainServices.CaseService.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Repositories.Entities;

[Table("Case", Schema = "dbo")]
internal class Case : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key]
    public long CaseId { get; set; }

    public int ProductTypeId { get; set; }
    public int State { get; set; }
    public DateTime StateUpdateTime { get; set; }

    // vlastnik case
    public int OwnerUserId { get; set; }
    public string? OwnerUserName { get; set; }

    // informace o klientovi
    public CIS.Foms.Enums.IdentitySchemes? CustomerIdentityScheme { get; set; }
    public int? CustomerIdentityId { get; set; }
    public string? FirstNameNaturalPerson { get; set; }
    public string Name { get; set; } = "";
    public DateOnly? DateOfBirthNaturalPerson { get; set; }
    public string? Cin { get; set; }

    // byznys data
    public string? ContractNumber { get; set; }

    [DataType("decimal(12,2)")]
    public decimal TargetAmount { get; set; }

    /// <summary>
    /// Vytvoreni entity z Create Requestu
    /// </summary>
    public static Case Create(long caseId, CreateCaseRequest request, DateTime now)
    {
        var entity = new Case
        {
            CaseId = caseId,

            StateUpdateTime = now,
            ProductTypeId = request.Data.ProductTypeId,

            Name = request.Customer.Name,
            FirstNameNaturalPerson = request.Customer.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = request.Customer.DateOfBirthNaturalPerson,

            TargetAmount = request.Data.TargetAmount,
            ContractNumber = request.Data.ContractNumber,

            OwnerUserId = request.CaseOwnerUserId
        };

        // pokud je zadany customer
        if (request.Customer is not null)
        {
            entity.CustomerIdentityScheme = (CIS.Foms.Enums.IdentitySchemes)Convert.ToInt32(request.Customer?.Identity?.IdentityScheme, System.Globalization.CultureInfo.InvariantCulture);
            entity.CustomerIdentityId = request.Customer?.Identity?.IdentityId;
        }

        return entity;
    }
}
