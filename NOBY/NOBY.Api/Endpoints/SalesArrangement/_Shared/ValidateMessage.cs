using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

public sealed class ValidateMessage
{
    /// <summary>
    /// Název parametru
    /// </summary>
    /// <example>Frantisek Novak, Doklad 1, chyba v poli Datum vydání</example>
    [Required]
    public string Parameter { get; set; } = string.Empty;

    /// <summary>
    /// Textace chyby
    /// </summary>
    /// <example>09.08.2022 Staré datum platnost od '01.09.2022' do '31.12.9999</example>
    [Required]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Severita chyby
    /// </summary>
    /// <example>Error</example>
    [Required]
    public MessageSeverity Severity { get; set; }
}

/// <summary>
/// Severita chyby
/// </summary>
public enum MessageSeverity
{
    Warning = 1,
    Error = 2
}