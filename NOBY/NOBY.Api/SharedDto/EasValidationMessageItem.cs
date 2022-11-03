using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.SharedDto;

public sealed class EasValidationMessageItem
{
    public string? Parameter { get; init; }
    public string? Value { get; init; }
    public int Code { get; init; }
    public string? Message { get; init; }
    public string? AdditionalInformation { get; init; }

    public EasValidationMessageItem() { }

    public EasValidationMessageItem(_SA.ValidationMessage sourceMsg)
    {
        Parameter = sourceMsg.Parameter;
        Value = sourceMsg.Value;
        Code = sourceMsg.Code;
        Message = sourceMsg.Message;
        AdditionalInformation = sourceMsg.AdditionalInformation;
    }
}