namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

public class ParametersMortgage
{
	public DateTime? ExpectedDateOfDrawing { get; set; }

	/// <example>CZK</example>
	public string? IncomeCurrencyCode { get; set; }

	/// <example>CZK</example>
	public string? ResidencyCurrencyCode { get; set; }
	
	/// <example>1</example>
	public int? ContractSignatureTypeId { get; set; }

    /// <example>1</example>
    public int? SalesArrangementSignatureTypeId { get; set; }

    public List<LoanRealEstateDto>? LoanRealEstates { get; set; }

	/// <summary>
	/// Zmocnenec - CustomerOnSAId
	/// </summary>
	public int? Agent { get; set; }

    /// <summary>
    /// Souhlas zmocněnce s elektronickou komunikací
    /// </summary>
    public bool? AgentConsentWithElCom { get; set; }
}

