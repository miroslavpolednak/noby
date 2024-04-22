﻿namespace ExternalServices.EasSimulationHT.Dto;

public sealed class MortgageExtraPaymentResult
{
    public bool IsExtraPaymentComplete { get; set; }
    public decimal ExtraPaymentAmount { get; set; }
    public decimal FeeAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal OtherUnpaidFees { get; set; }
    public decimal InterestOnLate { get; set; }
    public decimal InterestCovid { get; set; }
    public bool IsLoanOverdue { get; set; }
    public bool IsPaymentReduced { get; set; }
    public DateTime NewMaturityDate { get; set; }
    public decimal NewPaymentAmount { get; set; }
}