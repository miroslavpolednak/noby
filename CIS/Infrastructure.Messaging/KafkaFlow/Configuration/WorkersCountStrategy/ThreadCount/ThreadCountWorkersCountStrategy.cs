using System.Collections.Concurrent;
using System.Diagnostics;
using KafkaFlow;
using KafkaFlow.Configuration;
using Microsoft.Extensions.Logging;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.WorkersCountStrategy.ThreadCount;

internal sealed class ThreadCountWorkersCountStrategy : IWorkersCountStrategy
{
    private static readonly ConcurrentDictionary<string, int> _savedCounters = new();
    private readonly ThreadCountWorkerCountStrategyConfiguration _configuration;

    public ThreadCountWorkersCountStrategy(ThreadCountWorkerCountStrategyConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> GetWorkersCount(WorkersCountContext context, IDependencyResolver resolver)
    {
        var currentThreadCount = Process.GetCurrentProcess().Threads.Count;

        double ratio = (double)(currentThreadCount - _configuration.ThreadsMin) / (_configuration.ThreadsMax - _configuration.ThreadsMin);
        int workerCount = (int)(_configuration.WorkersMax - ratio * (_configuration.WorkersMax - _configuration.WorkersMin));

        workerCount = Math.Min(Math.Max(workerCount, _configuration.WorkersMin), _configuration.WorkersMax);

        if (_savedCounters.TryGetValue(context.ConsumerName, out int value) && value != workerCount)
        {
            var logger = resolver.Resolve<ILogger<ThreadCountWorkersCountStrategy>>(); 

            logger.LogInformation($"Worker count set to {workerCount} for ConsumerName {context.ConsumerName}");
        }

        _savedCounters[context.ConsumerName] = workerCount;

        return Task.FromResult(workerCount);
    }
}