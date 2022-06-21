using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.SendToCmp;

public sealed class SendToCmpResponse
{   
    /// <summary>
    /// Chyby bránící odeslání žádosti.
    /// </summary>
    public List<ValidationMessageItem> Errors { get; init; }

    /// <summary>
    /// Varování.
    /// </summary>
    public List<ValidationMessageItem> Warnings { get; init; }

    public SendToCmpResponse(_SA.ValidateSalesArrangementResponse source)
    {
        Errors = source.Errors.Select(i=> new ValidationMessageItem(i)).ToList();
        Warnings = source.Warnings.Select(i => new ValidationMessageItem(i)).ToList();
    }
}
