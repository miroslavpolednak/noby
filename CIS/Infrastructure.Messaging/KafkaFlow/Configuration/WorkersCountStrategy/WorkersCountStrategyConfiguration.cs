using CIS.Infrastructure.Messaging.KafkaFlow.Configuration.WorkersCountStrategy.ThreadCount;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.WorkersCountStrategy;

public class WorkersCountStrategyConfiguration
{
    public WorkersCountStrategy WorkersCountStrategy { get; set; } = WorkersCountStrategy.Default;

    public int EvaluationIntervalSecond { get; set; } = 90;

    public ThreadCountWorkerCountStrategyConfiguration? ThreadCountWorkerCount { get; set; }
}