namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class CreateCaseRequest
    : IRequest<CreateCaseResponse>, CIS.Core.Validation.IValidatableRequest
{
    /// <summary>
    /// ID dané úvěrové žádosti.
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public SystemId LoanApplicationIdMp { get; set; } = default!;

    /// <summary>
    /// ID business procesu, v rámci kterého, Risk Business Case vzniká.
    /// </summary>
    [DataMember(Order = 2)]
    public string ResourceProcessIdMp { get; set; } = default!;

    /// <summary>
    /// Typ zdrojové aplikace (např. NOBY, STARBUILD)
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public string ItChannel { get; set; } = default!;
}
