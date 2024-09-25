using KafkaFlow.Configuration;
using KafkaFlow;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration.WorkersCountStrategy;

internal interface IWorkersCountStrategy
{
    Task<int> GetWorkersCount(WorkersCountContext context, IDependencyResolver resolver);
}