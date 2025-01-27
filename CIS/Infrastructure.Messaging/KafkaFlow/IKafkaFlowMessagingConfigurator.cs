﻿using Confluent.SchemaRegistry;
using KafkaFlow;

namespace CIS.Infrastructure.Messaging.KafkaFlow;

public interface IKafkaFlowMessagingConfigurator
{
    IKafkaFlowMessagingConfigurator AddConsumerAvro<THandler>(string topic) where THandler : class, IMessageHandler;
    IKafkaFlowMessagingConfigurator AddConsumerAvro(string topic, Action<TypedHandlerConfigurationBuilder> handlers);
    IKafkaFlowMessagingConfigurator AddConsumerJson(string topic, Action<TypedHandlerConfigurationBuilder> handlers);
    IKafkaFlowMessagingConfigurator AddProducerAvro<TMessage>(string defaultTopic, SubjectNameStrategy subjectNameStrategy = SubjectNameStrategy.Record);
    IKafkaFlowMessagingConfigurator AddBatchConsumerAvro<TBatchHandler>(string topic) where TBatchHandler : class, IMessageMiddleware;
}