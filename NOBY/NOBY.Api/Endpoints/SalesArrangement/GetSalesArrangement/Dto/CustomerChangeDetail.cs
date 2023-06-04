using CIS.Foms.Types;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement.Dto;

public sealed class CustomerChangeDetail
{
    [Required]
    public List<CustomerChangeDetailApplicant>? Applicants { get; set; }

    [Required]
    public CustomerChangeDetailRelease? Release { get; set; }

    [Required]
    public CustomerChangeDetailAdd? Add { get; set; }

    [Required]
    public CustomerChangeDetailAgent? Agent { get; set; }

    /// <summary>
    /// Účet pro splácení
    /// </summary>
    [Required]
    public PaymentAccount? RepaymentAccount { get; set; }

    /// <summary>
    /// Komentář k žádosti o změnu
    /// </summary>
    [Required]
    public SalesArrangement.Dto.CommentToChangeRequest? CommentToChangeRequest { get; set; }
}

public sealed class CustomerChangeDetailApplicant
{
    public CustomerIdentity? Identity { get; set; }

    public CustomerChangeDetailNaturalPerson? NaturalPerson { get; set; }

    public NOBY.Dto.IdentificationDocumentBase? IdentificationDocument { get; set; }
}

public sealed class CustomerChangeDetailNaturalPerson
{
    /// <summary>
    /// Jméno
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Příjmení
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Datum narození
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
}

public sealed class CustomerChangeDetailRelease
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    public List<CustomerChangeDetailReleaseCustomer>? Customers { get; set; }
}

public sealed class CustomerChangeDetailReleaseCustomer
{
    public CustomerIdentity? Identity { get; set; }

    public CustomerChangeDetailNaturalPerson? NaturalPerson { get; set; }
}

public sealed class CustomerChangeDetailAdd
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    public List<CustomerChangeDetailAddCustomer>? Customers { get; set; }
}

public sealed class CustomerChangeDetailAddCustomer
{
    /// <summary>
    /// Jméno a příjmení
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Datum narození
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
}

public sealed class CustomerChangeDetailAgent
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Aktuální zmocněnec pro doručování
    /// </summary>
    [Required]
    public string ActualAgent { get; set; } = string.Empty;

    /// <summary>
    /// Nový zmocněnec pro doručování
    /// </summary>
    public string? NewAgent { get; set; }
}