using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KafkaService.Services
{
    public class KafkaProducer
    {
        public readonly string brokerURL;
        public readonly string clientID;

        public readonly string? groupSecondary;
        public readonly string topicPrimary;
        public readonly string? topicSecondary;
        private readonly ILogger<KafkaProducer> _logger;

        private KafkaProducer(ILogger<KafkaProducer> logger, KafkaConfiguration conf)
        {
            _logger = logger;
            brokerURL = conf.brokerURL;
            topicPrimary = conf.topic_primary;
            groupSecondary = conf.group_secondary;

            topicSecondary = conf.topic_secondary;
            clientID = conf.clientID + " " + Dns.GetHostName();
        }

        public static async Task<KafkaProducer> BuildProducer(ILogger<KafkaProducer> logger, KafkaConfiguration conf)
        {
            var kafkaProducer = new KafkaProducer(logger, conf);
            await kafkaProducer.checkTopic(conf.topic_primary);
            if (conf.group_secondary != null && conf.topic_secondary != null)
            {
                await kafkaProducer.checkTopic(conf.topic_secondary);
            }
            return kafkaProducer;
        }

        public async Task<KafkaData> MiniConsumer(Headers headers)
        {
            KafkaData kafkaData = new KafkaData(headers, InvocationType.reply, ResultStatus.unavailable);

            var conf = new ConsumerConfig
            {
                GroupId = groupSecondary,
                BootstrapServers = brokerURL,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = clientID,
                EnableAutoCommit = false,
            };
            using (var consumer = new ConsumerBuilder<string, string>(conf).Build())
            {
                _logger.LogInformation($"Waiting for reply in {clientID}");

                consumer.Subscribe(topicSecondary);

                await Task.Delay(2000).ContinueWith(t =>
                {
                    for (var loopCount = 0; loopCount < 4; ++loopCount)
                    {
                        var consumedValue = consumer.Consume(50);
                        if (consumedValue != null)
                        {
                            _logger.LogInformation($"Consuming reply in {clientID}");
                            var temp = new KafkaData(consumedValue.Message.Headers, consumedValue.Message.Key, consumedValue.Message.Value);
                            if (temp.GetUID() == kafkaData.GetUID())
                            {
                                kafkaData = temp;
                                consumer.Commit();

                                break;
                            }
                            consumer.Commit();
                        }
                        else
                        {
                            break;
                        }
                    }
                });

                return kafkaData;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="kafkaData">KafkaData's message key must be InvokeAndReturn.</param>
        /// <returns></returns>
        public async Task<KafkaData> SendAndReceiveData(KafkaData kafkaData)
        {
            if (kafkaData.message.Key != InvocationType.invokeAndReturn)
            {
                kafkaData.MarkError();
                return kafkaData;
            }

            await SendData(kafkaData);

            var result = await MiniConsumer(kafkaData.headers);
            return result;
        }

        public async Task SendData(KafkaData kafkaData)
        {
            var conf = new ProducerConfig
            {
                BootstrapServers = brokerURL,
                ClientId = clientID,
            };

            using (var producer = new ProducerBuilder<string, string>(conf).Build())
            {
                _logger.LogInformation($"Producing a message in {clientID}");
                await producer.ProduceAsync(topicPrimary, new Message<string, string> { Key = kafkaData.message.Key, Value = kafkaData.message.Value, Headers = kafkaData.headers });
            }
        }

        private async Task checkTopic(string topic)
        {
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
        }
    }
}