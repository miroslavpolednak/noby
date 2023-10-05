﻿namespace NOBY.Api.Endpoints.Offer.Dto;

public sealed class GetMortgageResponse
{
    /// <summary>
    /// ID simulace.
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// Unikatni identifikator pro session simulace.
    /// </summary>
    public string ResourceProcessId { get; set; } = null!;

    /// <summary>
    /// Zadani simulace.
    /// </summary>
    public MortgageInputsExtended SimulationInputs { get; set; } = null!;
    
    /// <summary>
    /// Vysledky simulace.
    /// </summary>
    public MortgageOutputs SimulationResults { get; set; } = null!;

    /// <summary>
    /// Datum, kdy končí garance pro danou simulaci
    /// </summary>
    public DateOnly? OfferGuaranteeDateTo { get; set; }

    public CreditWorthinessSimpleInputs? CreditWorthinessSimpleInputs { get; set; }
}
