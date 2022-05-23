﻿namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail.Dto;

public class MortgageDetailDto
{
    public int LoanKindId { get; set; }

    /// <summary>
    /// ČÍslo smlouvy. Odpovídá smlouvě uzavřené pro zřízení stavebního spoření, nebo hypotéky. Úvěry ze SS mají jiné číslo smlouvy (jiný suffix), ale nezobrazuje se na case.
    /// </summary>
    public string? ContractNumber { get; set; }

    /// <summary>
    /// Akce / produkt
    /// </summary>
    public string ProductName { get; set; } = null!;
    
    /// <summary>
    /// Výše úvěru
    /// </summary>
    public decimal LoanAmount { get; set; }
    
    /// <summary>
    /// Skládačková úroková sazba 
    /// </summary>
    public decimal LoanInterestRate { get; set; }
    
    /// <summary>
    /// Datum uzavreni smlouvy
    /// </summary>
    public DateTime? ContractStartDate { get; set; }
    
    /// <summary>
    /// Datum fixace
    /// </summary>
    public DateTime? FixationDate { get; set; }

    /// <summary>
    /// Zustatek na ucte
    /// </summary>
    public decimal? AccountBalance { get; set; }
    
    /// <summary>
    /// Zbyva cerpat
    /// </summary>
    public decimal? AmountToWithdraw { get; set; }
    
    /// <summary>
    /// Mesicni splatka
    /// </summary>
    public decimal MonthlyPayment { get; set; }

    /// <summary>
    /// Datum cerpani
    /// </summary>
    public DateTime? DateOfDrawing { get; set; }
    
    /// <summary>
    /// Platne VUP od
    /// </summary>
    public DateTime? LoanTermsValidFrom { get; set; }
    
    /// <summary>
    /// Rocni vypis z uctu
    /// </summary>
    public bool YearlyAccountStatement { get; set; }

    //TODO tohle musi nekdo prelozit
    public int? Jistina { get; set; }
    public int? DruhUveru { get; set; }
    public int? DelkaFixaceUrokoveSazby { get; set; }
    public string? UcelUveru { get; set; }
    public DateTime? DatumPrvniVyplatyZUveru { get; set; }
    public DateTime? DatumPredpSplatnosti { get; set; }
}