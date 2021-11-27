using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaService.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaService.Services
{
    public class KafkaConsumer : BackgroundService
    {
        public readonly string brokerURL;
        public readonly string clientID;
        public readonly string groupPrimary;
        public readonly string topicPrimary;
        public readonly string? topicSecondary;
        private readonly ILogger<KafkaConsumer> _logger;

        private Func<KafkaData, Task> consume;

        private Func<KafkaData, Task<KafkaData>>? produce;

        public KafkaConsumer(ILogger<KafkaConsumer> logger, KafkaConfiguration conf, Func<KafkaData, Task> consumerMethod, Func<KafkaData, Task<KafkaData>>? produce = null)
        {
            _logger = logger;
            brokerURL = conf.brokerURL;
            topicPrimary = conf.topic_primary;
            groupPrimary = conf.group_primary;
            topicSecondary = conf.topic_secondary;
            consume = consumerMethod;
            clientID = conf.clientID + " " + Dns.GetHostName();
            this.produce = produce;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Background service for {clientID} is stopping.");

            await base.StopAsync(stoppingToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Background service started for {clientID}");
            await checkTopic(topicPrimary);
            if (topicSecondary != null)
            {
                await checkTopic(topicSecondary);
            }
            try
            {
                var task = Task.Run(async () =>
                {
                    await KafkaEventHandler(stoppingToken);
                }, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error encountered in starting consumer for {clientID}: {e}");
            }
        }

        private async Task checkTopic(string topic)
        {
            _logger.LogInformation($"Background service for {clientID} is checking topic.");
            var adminClient = new AdminClientBuilder(new AdminClientConfig() { BootstrapServers = brokerURL });
            using (var buildClient = adminClient.Build())
            {
                var metadata = buildClient.GetMetadata(TimeSpan.FromSeconds(10));
                var topicsMetadata = metadata.Topics;
                var topicNames = metadata.Topics.Select(topicMetadata => topicMetadata.Topic);
                if (!topicNames.Contains(topic))
                {
                    await buildClient.CreateTopicsAsync(new TopicSpecification[] { new TopicSpecification { Name = topic, ReplicationFactor = 1, NumPartitions = 1 } });
                }
            }
            _logger.LogInformation($"Background service for {clientID} has finished checking topics.");
        }

        private async Task KafkaEventHandler(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{clientID} has started KafkaEventHandler");
            var conf = new ConsumerConfig
            {
                GroupId = groupPrimary,
                BootstrapServers = brokerURL,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = clientID,
                EnableAutoCommit = false,
            };
            using (var consumer = new ConsumerBuilder<string, string>(conf).Build())
            {
                _logger.LogInformation($"Consumer Loop starting in {clientID}. Note: Messages will be silently discarded if either key or value is null");
                while (!cancellationToken.IsCancellationRequested)
                {
                    consumer.Subscribe(topicPrimary);

                    var consumedValue = consumer.Consume(100);
                    if (consumedValue != null)
                    {
                        _logger.LogInformation($"Consuming a message in {clientID}");

                        var kafkaData = new KafkaData(consumedValue.Message.Headers, consumedValue.Message.Key, consumedValue.Message.Value);

                        if (consumedValue.Message.Key == InvocationType.justInvoke)
                        {
                            await consume(kafkaData);
                        }
                        else if (consumedValue.Message.Key == InvocationType.invokeAndReturn)
                        {
                            if (produce != null && topicSecondary != null)
                            {
                                var result = await produce(kafkaData);

                                await MiniProducer(result);
                            }
                        }
                        consumer.Commit();
                    }

                    await Task.Delay(500, cancellationToken);
                }

                consumer.Unsubscribe();
                consumer.Close();
                consumer.Dispose();
            }

            _logger.LogInformation($"{clientID} has stopped KafkaEventHandler");
        }

        private async Task MiniProducer(KafkaData kafkaData)
        {
            _logger.LogInformation($"{clientID} producing a reply.");
            var conf = new ProducerConfig
            {
                BootstrapServers = brokerURL,
                ClientId = clientID,
            };

            using (var producer = new ProducerBuilder<string, string>(conf).Build())
            {
                _logger.LogInformation($"{clientID} producing a reply.");
                await producer.ProduceAsync(topicSecondary, new Message<string, string> { Key = kafkaData.message.Key, Value = kafkaData.message.Value, Headers = kafkaData.headers });
            }
        }
    }
}