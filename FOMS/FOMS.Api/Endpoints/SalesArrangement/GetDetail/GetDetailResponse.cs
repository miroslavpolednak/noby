using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

public sealed class GetDetailResponse
{
    public int ProductTypeId { get; set; }

    /// <summary>
    /// ID zadosti.
    /// </summary>
    public int SalesArrangementId { get; set; }
    
    /// <summary>
    /// Druh zadosti. Ciselnik SalesArrangementTypes.
    /// </summary>
    public int SalesArrangementTypeId { get; set; }

    public string? LoanApplicationAssessmentId { get; set; }

    /// <summary>
    /// Datum vytvoreni SA
    /// </summary>
    public DateTime CreatedTime { get; set; }
	
    /// <summary>
    /// Jmeno a prijmeni uzivatele, ktery vytvoril SA
    /// </summary>
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// Data o zadosti - bude se jednat o ruzne objekty podle typu zadosti.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Dalsi udaje o pripadu/uveru. Typ objektu je podle typu SA.
    /// </summary>
    /// <remarks>
    /// OneOf(
    /// DomainServices.SalesArrangementService.Contracts.SalesArrangementParametersMortgage
    /// )
    /// </remarks>
    public SalesArrangement.Dto.ParametersMortgage? Parameters { get; set; }
}