namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.WorkersCountStrategy.ThreadCount;

public class ThreadCountWorkerCountStrategyConfiguration
{
    public int ThreadsMin { get; set; }

    public int ThreadsMax { get; set; }

    public int WorkersMin { get; set; }

    public int WorkersMax { get; set; }
}