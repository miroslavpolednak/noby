namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail.Dto;

public class MortgageDetailDto
{
    /// <summary>
    /// ČÍslo smlouvy. Odpovídá smlouvě uzavřené pro zřízení stavebního spoření, nebo hypotéky. Úvěry ze SS mají jiné číslo smlouvy (jiný suffix), ale nezobrazuje se na case.
    /// </summary>
    public string? ContractNumber { get; set; }
    
    // aakce produkt???
    
    /// <summary>
    /// Výše úvěru
    /// </summary>
    public decimal LoanAmount { get; set; }
    
    /// <summary>
    /// Skládačková úroková sazba (číselníková sazba mínus slevy dané marketingovými akcemi). 
    /// </summary>
    public decimal InterestRate { get; set; }
    
    /// <summary>
    /// Datum uzavreni???
    /// </summary>
    public DateTime? ExpectedDateOfDrawing { get; set; }
    
    /// <summary>
    /// Fixace???
    /// </summary>
    public int FixationPeriod { get; set; }
    
    
}