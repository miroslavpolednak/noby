using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.SendToCmp;

public sealed class ValidationMessageItem
{
    public string Parameter { get; init; }
    public string Value { get; init; }
    public int Code { get; init; }
    public string Message { get; init; }
    public string AdditionalInformation { get; init; }

    public ValidationMessageItem(_SA.ValidationMessage sourceMsg)
    {
        Parameter = sourceMsg.Parameter;
        Value = sourceMsg.Value;
        Code = sourceMsg.Code;
        Message = sourceMsg.Message;
        AdditionalInformation = sourceMsg.AdditionalInformation;
    }
}