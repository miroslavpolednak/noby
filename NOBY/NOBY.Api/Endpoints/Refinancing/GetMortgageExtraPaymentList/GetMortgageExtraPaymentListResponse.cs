﻿namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPaymentList;

public sealed class GetMortgageExtraPaymentListResponse
{
    /// <summary>
    /// datum vytvoření
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Způsob splacení
    /// </summary>
    public bool IsExtraPaymentFullyRepaid { get; set; }

    /// <summary>
    /// Výše splátky
    /// </summary>
    public decimal? ExtraPaymentAmount { get; set; }

    /// <summary>
    /// Výše splacené jistiny
    /// </summary>
    public decimal? PrincipalAmount { get; set; }

    /// <summary>
    /// Datum splátky
    /// </summary>
    public DateOnly ExtraPaymentDate { get; set; }

    /// <summary>
    /// Stav
    /// </summary>
    public int RefinancingStateId { get; set; }

    /// <summary>
    /// Uhrazeno; 0 - Unknown; 1 - Uhrazeno; 2 - Neuhrazeno
    /// </summary>
    public int PaymentState { get; set; }
}