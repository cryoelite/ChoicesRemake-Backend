using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KafkaService.Services
{
    public class KafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;

        public readonly string brokerURL;
        public readonly string topic;
        public readonly string group;
        public readonly string clientID;

        public static async Task<KafkaProducer> BuildProducer(ILogger<KafkaProducer> logger, KafkaConfiguration conf)
        {
            var kafkaProducer = new KafkaProducer(logger, conf);
            await kafkaProducer.checkTopic();
            return kafkaProducer;
        }

        private KafkaProducer(ILogger<KafkaProducer> logger, KafkaConfiguration conf)
        {
            _logger = logger;
            brokerURL = conf.brokerURL;
            topic = conf.topic;
            group = conf.group;
            clientID = conf.clientID + " " + Dns.GetHostName();
        }

        private async Task checkTopic()
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

        public async Task SendData(KeyValuePair<string, string> message, Dictionary<string, string>? headers)
        {
            var conf = new ProducerConfig
            {
                BootstrapServers = brokerURL,
                ClientId = clientID,
            };
            var _headers = new Headers();
            if (headers != null)
            {

                foreach (var keyValuePair in headers)
                {
                    var byteArray = Encoding.UTF8.GetBytes(keyValuePair.Value);

                    _headers.Add(keyValuePair.Key, byteArray);
                }

            }

            using (var producer = new ProducerBuilder<string, string>(conf).Build())
            {
                await producer.ProduceAsync(topic, new Message<string, string> { Key = message.Key, Value = message.Value, Headers = headers == null ? null : _headers });
            }
        }
    }
}