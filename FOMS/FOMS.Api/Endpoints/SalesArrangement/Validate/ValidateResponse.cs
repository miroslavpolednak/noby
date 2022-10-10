using _SA = DomainServices.SalesArrangementService.Contracts;
using FOMS.Api.SharedDto;

namespace FOMS.Api.Endpoints.SalesArrangement.Validate;

public sealed class ValidateResponse
{
    /// <summary>
    /// Chyby bránící odeslání žádosti.
    /// </summary>
    public List<EasValidationMessageItem>? Errors { get; set; }

    /// <summary>
    /// Varování.
    /// </summary>
    public List<EasValidationMessageItem>? Warnings { get; set; }

    public ValidateResponse(_SA.ValidateSalesArrangementResponse source)
    {
        Errors = source.Errors.Select(i => new EasValidationMessageItem(i)).ToList();
        Warnings = source.Warnings.Select(i => new EasValidationMessageItem(i)).ToList();
    }
}
