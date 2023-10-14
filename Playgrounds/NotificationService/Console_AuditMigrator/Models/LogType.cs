namespace Console_AuditMigrator.Models;

public enum LogType
{
    ReceivedHttpRequest = 0,
    SendingHttpResponse = 1,
    ProducingToKafka = 2,
    ProducedToKafka = 3,
    CouldNotProduceToKafka = 4,
    ReceivedReport = 5
}