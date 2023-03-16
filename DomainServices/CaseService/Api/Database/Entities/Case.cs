using DomainServices.CaseService.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Database.Entities;

[Table("Case", Schema = "dbo")]
internal sealed class Case 
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key]
    public long CaseId { get; set; }

    public int ProductTypeId { get; set; }
    public int State { get; set; }
    public DateTime StateUpdateTime { get; set; }
    public byte StateUpdatedInStarbuild { get; set; }

    // vlastnik case
    public int OwnerUserId { get; set; }
    public string? OwnerUserName { get; set; }

    // informace o klientovi
    public CIS.Foms.Enums.IdentitySchemes? CustomerIdentityScheme { get; set; }
    public long? CustomerIdentityId { get; set; }
    public string? FirstNameNaturalPerson { get; set; }
    public string Name { get; set; } = "";
    public DateOnly? DateOfBirthNaturalPerson { get; set; }
    public string? Cin { get; set; }
    public string? EmailForOffer { get; set; }
    public string? PhoneNumberForOffer { get; set; }
    public string? PhoneIDCForOffer { get; set; }

    // byznys data
    public string? ContractNumber { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal TargetAmount { get; set; }

    public bool? IsEmployeeBonusRequested { get; set; }

    public List<ActiveTask>? ActiveTasks { get; set; }

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
            Cin = request.Customer.Cin,

            TargetAmount = request.Data.TargetAmount,
            ContractNumber = request.Data.ContractNumber,
            IsEmployeeBonusRequested = request.Data.IsEmployeeBonusRequested,

            OwnerUserId = request.CaseOwnerUserId,
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
