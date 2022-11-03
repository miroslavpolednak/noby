using NOBY.Api.SharedDto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp;

public sealed class SendToCmpResponse
{   
    /// <summary>
    /// Chyby bránící odeslání žádosti.
    /// </summary>
    public List<EasValidationMessageItem> Errors { get; init; }

    /// <summary>
    /// Varování.
    /// </summary>
    public List<EasValidationMessageItem> Warnings { get; init; }

    public SendToCmpResponse(_SA.ValidateSalesArrangementResponse source)
    {
        Errors = source.Errors.Select(i=> new EasValidationMessageItem(i)).ToList();
        Warnings = source.Warnings.Select(i => new EasValidationMessageItem(i)).ToList();
    }
}
