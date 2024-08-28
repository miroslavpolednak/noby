using CIS.Infrastructure.Messaging.KafkaFlow.Configuration.WorkersCountStrategy.ThreadCount;
using KafkaFlow;
using KafkaFlow.Configuration;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.WorkersCountStrategy;

internal static class WorkersCountStrategyFactory
{
    public static Func<WorkersCountContext, IDependencyResolver, Task<int>> CreateCalculator(WorkersCountStrategyConfiguration config)
    {
        var strategy = config.WorkersCountStrategy switch
        {
            WorkersCountStrategy.ThreadCount => new ThreadCountWorkersCountStrategy(config.ThreadCountWorkerCount!),
            _ => throw new NotImplementedException()
        };

        return strategy.GetWorkersCount;
    }
}