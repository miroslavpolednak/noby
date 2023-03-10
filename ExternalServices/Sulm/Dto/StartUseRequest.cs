namespace ExternalServices.Sulm.Dto;

internal sealed class StartUseRequest
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string clientId { get; set; }
    public string purposeCode { get; set; }
    public string channelCode { get; set; }
    public string userId { get; set; }
    public string userIdType { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
