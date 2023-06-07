namespace NOBY.Dto.Workflow;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public class AmendmentsPriceException
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    /// <summary>
    /// Cenová výjimka: Platnost cenové výjimky
    /// </summary>
    /// <example>24.12.2023</example>
    [Required]
    [MinLength(1)]
    public DateOnly Expiration { get; set; }

    [Required]
    public LoanInterestRates LoanInterestRate { get; set; } = null!;

    public List<Fee>? Fees { get; set; } = null!;
    
    /// <summary>
    /// Rozhodnutí
    /// </summary>
    /// <example>Schváleno</example>
    public string Decision { get; set; } = null!;
}

public class LoanInterestRates
{
    /// <summary>
    /// Nabídková sazba
    /// </summary>
    /// <example>2,49 %</example>
    [Required]
    public decimal LoanInterestRate { get; set; }
    
    /// <summary>
    /// Poskytnutá sazba
    /// </summary>
    /// <example>2 %</example>
    [Required]
    public decimal LoanInterestRateProvided { get; set; }
    
    /// <summary>
    /// Typ sazby
    /// </summary>
    /// <example>standardní</example>
    [Required]
    public string LoanInterestRateAnnouncedTypeName { get; set; } = null!;

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0,49 %</example>
    public decimal? LoanInterestRateDiscount { get; set; }
}

public class Fee
{
    /// <summary>
    /// Název poplatku
    /// </summary>
    /// <example>Zpracování žádosti</example>
    public string FeeId { get; set; } = null!;
    
    /// <summary>
    /// Sazebníková cena (Kč)
    /// </summary>
    /// <example>1000</example>
    public decimal TariffSum { get; set; }
    
    /// <summary>
    /// Navrhovaná cena (Kč)
    /// </summary>
    /// <example>500</example>
    public decimal FinalSum { get; set; }
    
    /// <summary>
    /// Sleva (%)
    /// </summary>
    /// <example>50</example>
    public decimal DiscountPercentage { get; set; }
}