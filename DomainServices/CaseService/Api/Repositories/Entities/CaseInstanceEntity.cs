using DomainServices.CaseService.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Repositories.Entities;

[Table("CaseInstance", Schema = "dbo")]
internal class CaseInstance : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key]
    public long CaseId { get; set; }

    public int ProductInstanceTypeId { get; set; }
    public int State { get; set; }
    public DateTime StateUpdateTime { get; set; }

    // vlastnik case
    public int OwnerUserId { get; set; }
    public string? OwnerUserName { get; set; }

    // informace o klientovi
    public CIS.Core.IdentitySchemes? CustomerIdentityScheme { get; set; }
    public int? CustomerIdentityId { get; set; }
    public string? FirstNameNaturalPerson { get; set; }
    public string Name { get; set; } = "";
    public DateOnly? DateOfBirthNaturalPerson { get; set; }
    public string? Cin { get; set; }

    // byznys data
    public string? ContractNumber { get; set; }
    public int? TargetAmount { get; set; }
    public bool IsActionRequired { get; set; }

    /// <summary>
    /// Vytvoreni entity z Create Requestu
    /// </summary>
    public static CaseInstance Create(long caseId, CreateCaseRequest request)
    {
        var entity = new CaseInstance
        {
            CaseId = caseId,

            StateUpdateTime = DateTime.Now,
            ProductInstanceTypeId = request.Data.ProductInstanceTypeId,

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
            entity.CustomerIdentityScheme = (CIS.Core.IdentitySchemes)Convert.ToInt32(request.Customer?.Identity?.IdentityScheme);
            entity.CustomerIdentityId = request.Customer?.Identity?.IdentityId;
        }

        return entity;
    }
}
