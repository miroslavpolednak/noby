namespace DomainServices.CustomerService.Api.Repositories.Entities;

internal record Partner
{
    public Partner()
    {
        Kontakty = new List<PartnerKontakt>();
    }

    public int Id { get; init; }

    public string? RodneCisloIco { get; init; }

    public string? Titul { get; init; }

    public string? TitulZa { get; init; }

    public string? Jmeno { get; init; }

    public string? Prijmeni { get; init; }

    public DateTime? DatumNarozeni { get; init; }

    public string? MistoNarozeni { get; init; }

    public int Pohlavi { get; init; }

    public int ZdrojDat { get; init; }

    public int TypDokladu { get; init; }

    public DateTime? PreukazPlatnostDo { get; init; }

    public string? PrukazTotoznosti { get; init; }

    public string? PrukazVydal { get; init; }

    public int? PrukazStatVydaniId { get; init; }

    public DateTime? PrukazVydalDatum { get; init; }

    public string? Ulice { get; init; }

    public string? VypisyUlice { get; init; }

    public string? CisloDomu2 { get; init; }

    public string? VypisyCisloDomu2 {  get; init; }

    public string? CisloDomu4 {  get; init; }

    public string? VypisyCisloDomu4 {  get; init; }

    public string? Psc { get; init; }

    public string? VypisyPsc {  get; init; }

    public string? Misto {  get; init; }

    public string? VypisyMisto {  get; init; }

    public List<PartnerKontakt> Kontakty { get; init; }
}
