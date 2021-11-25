namespace FOMS.Api.Endpoints.Offer.Dto;

/// <summary>
/// Vstup do simulace stavebniho sporeni s/bez uveru
/// </summary>
internal class BuildingSavingsInput
{
    /// <summary>
    /// Kód produktu. V UI pojmenováno jako tarif.
    /// </summary>
    public int? ProductCode { get; set; }

    /// <summary>
    /// Cílová částka v CZK 
    /// </summary>
    public int? TargetAmount { get; set; }

    /// <summary>
    /// Obchodní akce 
    /// </summary>
    public int? ActionCode { get; set; }

    public bool IsWithLoan { get; set; }

    /// <summary>
    /// Uverova akce
    /// </summary>
    public int? LoanActionCode { get; set; }

    /// <summary>
    /// True pokud se jedna o fyzickou osobu
    /// </summary>
    public bool ClientIsNaturalPerson { get; set; }

    public bool StateSubsidy { get; set; }

    /// <summary>
    /// True pokud se jedna o spolecenstvi vlastniku
    /// </summary>
    public bool ClientIsSVJ { get; set; }

    public static implicit operator BuildingSavingsInput(DomainServices.OfferService.Contracts.BuildingSavingsInput data)
        => new()
        {
            ClientIsSVJ = data.ClientIsSVJ,
            ActionCode = data.ActionCode,
            TargetAmount = data.TargetAmount,
            ClientIsNaturalPerson = data.ClientIsNaturalPerson,
            ProductCode = data.ProductCode,
            LoanActionCode = data.LoanActionCode,
            StateSubsidy = data.StateSubsidy,
            IsWithLoan = data.IsWithLoan
        };

    public static implicit operator DomainServices.OfferService.Contracts.SimulateBuildingSavingsRequest(BuildingSavingsInput data)
        => new()
        {
            InputData = new DomainServices.OfferService.Contracts.BuildingSavingsInput
            {
                ClientIsSVJ = data.ClientIsSVJ,
                ActionCode = data.ActionCode.GetValueOrDefault(),
                TargetAmount = data.TargetAmount.GetValueOrDefault(),
                ClientIsNaturalPerson = data.ClientIsNaturalPerson,
                ProductCode = data.ProductCode.GetValueOrDefault(),
                LoanActionCode = data.LoanActionCode,
                StateSubsidy = data.StateSubsidy,
                IsWithLoan = data.IsWithLoan
            }
        };
}
