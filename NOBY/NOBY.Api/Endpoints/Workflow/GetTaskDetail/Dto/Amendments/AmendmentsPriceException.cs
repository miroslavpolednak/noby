﻿using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Workflow.GetTaskDetail.Dto.Amendments;

public class AmendmentsPriceException
{
    /// <summary>
    /// Cenová výjimka: Platnost cenové výjimky
    /// </summary>
    /// <example>24.12.2023</example>
    [Required]
    public DateTime Expiration { get; set; }

    [Required]
    public LoanInterestRates LoanInterestRate { get; set; } = null!;

    public List<Fee> Fees { get; set; } = null!;
    
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
    public string LoanInterestRate { get; set; } = null!;
    
    /// <summary>
    /// Poskytnutá sazba
    /// </summary>
    /// <example>2 %</example>
    [Required]
    public string LoanInterestRateProvided { get; set; } = null!;
    
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
    public string LoanInterestRateDiscount { get; set; } = null!;
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
    public int TariffSum { get; set; }
    
    /// <summary>
    /// Navrhovaná cena (Kč)
    /// </summary>
    /// <example>500</example>
    public int FinalSum { get; set; }
    
    /// <summary>
    /// Sleva (%)
    /// </summary>
    /// <example>50</example>
    public int DiscountPercentage { get; set; }
}