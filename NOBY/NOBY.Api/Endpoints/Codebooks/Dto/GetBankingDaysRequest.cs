using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Codebooks.Dto;

public sealed class GetBankingDaysRequest
{
    /// <summary>
    /// Ohraničení vrácených dat datumem od
    /// </summary>
    [Required]
    public DateOnly DateFrom { get; set; }

    /// <summary>
    /// Ohraničení vrácených dat datumem do
    /// </summary>
    [Required]
    public DateOnly DateTo { get; set; }
}
