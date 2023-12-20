namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public sealed class HouseholdObligationItem
{
    public int Id { get; set; }

    /// <summary>
    /// Typ závazku
    /// </summary>
    public int ObligationTypeId { get; set; }

    /// <summary>
    /// Typ závazku - název
    /// </summary>
    public string ObligationTypeName { get; set; } = string.Empty;

    /// <summary>
    /// Stav závazku
    /// </summary>
    public int ObligationSourceId { get; set; }

    /// <summary>
    /// Věřitel
    /// </summary>
    public string CreditorName { get; set; } = string.Empty;

    /// <summary>
    /// Nesplacená jistina
    /// </summary>
    public Amount? LoanPrincipalAmount { get; set; }

    /// <summary>
    /// Splátka
    /// </summary>
    public Amount? InstallmentAmount { get; set; }

    /// <summary>
    /// Limit
    /// </summary>
    public Amount? CreditCardLimit { get; set; }

    /// <summary>
    /// Korekce
    /// </summary>
    public int? CorrectionTypeId { get; set; }

    /// <summary>
    /// Kategorie závazku
    /// </summary>
    public int ObligationLaExposureId { get; set; }

    /// <summary>
    /// Kategorie závazku - název
    /// </summary>
    public string ObligationLaExposureName { get; set; }

    /// <summary>
    /// Závazek FOP
    /// </summary>
    public bool ZavazekFOP { get; set; }

    public List<string>? Codebtors { get; set; }

    /// <summary>
    /// Kód skupiny
    /// </summary>
    public string? KbGroupInstanceCode { get; set; }

    /// <summary>
    /// Vyčerpaná částka
    /// </summary>
    public int DrawingAmount { get; set; }

    /// <summary>
    /// Číslo  úvěrového účtu
    /// </summary>
    public string? BankAccount { get; set; }

    /// <summary>
    /// Datum poskytnutí
    /// </summary>
    public DateTime? ContractDate { get; set; }

    /// <summary>
    /// Datum splatnosti
    /// </summary>
    public DateTime? MaturityDate { get; set; }

    /// <summary>
    /// Datum aktualizace dat v CBCB
    /// </summary>
    public DateTime? CbcbDataLastUpdate { get; set; }

    public sealed class Amount
    {
        /// <summary>
        /// Částka před korekcí
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Částka po korekci
        /// </summary>
        public int? Correction { get; set; }

        /// <summary>
        /// Kód měny
        /// </summary>
        public string CurrencyCode { get; set; } = string.Empty;
    }
}

