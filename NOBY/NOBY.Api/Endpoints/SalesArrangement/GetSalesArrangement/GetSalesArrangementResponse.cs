using NOBY.Api.Endpoints.SalesArrangement.Dto;
using NOBY.Dto.Attributes;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement;

public sealed class GetSalesArrangementResponse
{
    public int ProductTypeId { get; set; }

    /// <summary>
    /// ID zadosti.
    /// </summary>
    public int SalesArrangementId { get; set; }

    /// <summary>
    /// Stav žádosti. Číselník SalesArrangementState.
    /// </summary>
    public int State { get; set; }

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
    /// Rozhodný den sazby (datum založení = datum garance)
    /// </summary>
    public DateTime OfferGuaranteeDateFrom { get; set; }

    /// <summary>
    /// Datum, kdy končí garance pro danou simulaci
    /// </summary>
    public DateTime OfferGuaranteeDateTo { get; set; }

    /// <summary>
    /// Dalsi udaje o pripadu/uveru. Typ objektu je podle typu SA.
    /// </summary>
    /// <remarks>
    /// OneOf(
    /// DomainServices.SalesArrangementService.Contracts.SalesArrangementParametersMortgage
    /// DomainServices.SalesArrangementService.Contracts.SalesArrangementParametersDrawing
    /// DomainServices.SalesArrangementService.Contracts.SalesArrangementParametersGeneralChange
    /// DomainServices.SalesArrangementService.Contracts.SalesArrangementParametersHUBN
    /// DomainServices.SalesArrangementService.Contracts.SalesArrangementParametersCustomerChange
    /// DomainServices.SalesArrangementService.Contracts.SalesArrangementParametersCustomerChange3602
    /// )
    /// </remarks>
    [SwaggerOneOf(typeof(ParametersMortgage),
                  typeof(ParametersDrawing),
                  typeof(Dto.HUBNDetail),
                  typeof(Dto.GeneralChangeDetail),
                  typeof(Dto.CustomerChangeDetail),
                  typeof(Dto.CustomerChange3602Detail))]
    public object? Parameters { get; set; }
}