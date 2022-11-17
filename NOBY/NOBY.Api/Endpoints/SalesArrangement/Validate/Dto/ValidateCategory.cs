using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.Validate.Dto;

public sealed class ValidateCategory
{
    /// <summary>
    /// Jméno kategorie
    /// </summary>
    /// <example>Klient</example>
    [Required]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public List<ValidateMessage> ValidationMessages { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
