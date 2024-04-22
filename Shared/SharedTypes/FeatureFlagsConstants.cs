namespace SharedTypes;

public static class FeatureFlagsConstants
{
    public const string FeatureFlagsSection = "FeatureManagement";

    /// <summary>
    /// Flag oznacujici zda jsou zapnute modre produkty nebo ne (TRUE jsou zapnute)
    /// </summary>
    public const string BlueBang = "BlueBang";

    /// <summary>
    /// zapíná/vypíná elektronické podepisování
    /// </summary>
    public const string ElectronicSigning = "ElectronicSigning";

    /// <summary>
    /// zapíná/vypíná elektronické podepisování workflow úkolů
    /// </summary>
    public const string ElectronicWorkflowSigning = "ElectronicWorkflowSigning";

    /// <summary>
    /// zapíná/vypíná retenční endpointy
    /// </summary>
    public const string Retention = "Retention";

    /// <summary>
    /// zapíná/vypíná refixační endpointy
    /// </summary>
    public const string Refixation = "Refixation";

    /// <summary>
    /// zapíná/vypíná endpointy související s mimořádnou splátkou
    /// </summary>
    public const string ExtraPayment = "ExtraPayment";

    /// <summary>
    /// zapíná/vypíná endpointy související s interním refinancováním
    /// </summary>
    public const string InternalRefinancing = "InternalRefinancing";
}
