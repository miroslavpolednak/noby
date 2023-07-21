namespace ExternalServices.ESignatures.Dto;

public sealed class UserInfo
{
    public string Cpm { get; set; } = string.Empty;

    public string Icp { get; set; } = string.Empty;

    /// <summary>
    /// "Name" z ePodpis kontraktu
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}

