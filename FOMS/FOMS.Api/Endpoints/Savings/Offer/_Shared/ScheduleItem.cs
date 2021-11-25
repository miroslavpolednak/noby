namespace FOMS.Api.Endpoints.Offer.Dto;

internal class ScheduleItem
{
    public DateTime? Date { get; set; }

    public int Sum { get; set; }

    public int Balance { get; set; }

    public string? Note { get; set; } = null;

    public string? Info { get; set; } = null;

    public static implicit operator ScheduleItem(DomainServices.OfferService.Contracts.ScheduleItem data)
        => new ScheduleItem
        {
            Sum = data.Sum,
            Balance = data.Balance,
            Date = data.Date,
            Info = data.Info,
            Note = data.Note
        };
}
