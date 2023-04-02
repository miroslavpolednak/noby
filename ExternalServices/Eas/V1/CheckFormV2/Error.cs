namespace ExternalServices.Eas.V1.CheckFormV2;

public sealed class ErrorDto
{
    public string Value { get; set; } = string.Empty;

    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public string AdditionalInformation { get; set; } = string.Empty;

    public string ErrorQueue { get; set; } = string.Empty;

    public bool Severity { get; set; }
}
