namespace CIS.Infrastructure.Messaging;

internal static class Constants
{
    /// <summary>
    /// Nazev elementu v appsettings.json, ve kterem je konfigurace Kafky
    /// </summary>
    internal const string KafkaConfigurationElement = "Kafka";

    /// <summary>
    /// Nazev root element v appsettings.json pod ktery jsou konfigurace Messagingu
    /// </summary>
    internal const string MessagingConfigurationElement = "CisMessaging";
}
