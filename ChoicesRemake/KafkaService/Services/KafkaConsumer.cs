using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaService.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaService.Services
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private Func<KeyValuePair<string, string>, Dictionary<string, string>?, Task> consume;
        public readonly string brokerURL;
        public readonly string topic;
        public readonly string group;
        public readonly string clientID;
        public ConcurrentBag<int> useCount = new ConcurrentBag<int>();

        public KafkaConsumer(ILogger<KafkaConsumer> logger, KafkaConfiguration conf, Func<KeyValuePair<string, string>, Dictionary<string, string>?, Task> consumerMethod)
        {
            _logger = logger;
            brokerURL = conf.brokerURL;
            topic = conf.topic;
            group = conf.group;
            consume = consumerMethod;
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

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await checkTopic();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Background service started for {clientID}");
            int val;
            useCount.TryTake(out val);
            val += 1;
            useCount.Add(val);
            
            {
                try
                {
                    await KafkaEventHandler(stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error encountered in starting consumer for {clientID}: {e}");
                }
            }
        }

        private async Task KafkaEventHandler(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = group,
                BootstrapServers = brokerURL,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = clientID,
            };
            using (var consumer = new ConsumerBuilder<string, string>(conf).Build())
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    int val;
                    useCount.TryTake(out val);
                    val += 1;
                    useCount.Add(val);
                    var consumedValue = consumer.Consume(cancellationToken);
                    if (consumedValue.Message != null && consumedValue.Message.Key != null && consumedValue.Message.Key != null)
                    {
                        _logger.LogInformation($"Consumed a message in {clientID}");
                        var headers = consumedValue.Message.Headers;
                        if (headers != null)
                        {
                            var _headers = new Dictionary<string, string>();
                            foreach (var elem in headers) 
                            {
                                var value = elem.GetValueBytes();
                                var stringData = Encoding.UTF8.GetString(value);
                                _headers.Add(elem.Key,stringData);
                            }
                            await consume(new KeyValuePair<string, string>(consumedValue.Message.Key, consumedValue.Message.Value), _headers);
                        }
                        else
                        {
                            await consume(new KeyValuePair<string, string>(consumedValue.Message.Key, consumedValue.Message.Value), null);
                        }
                    }
                    else if (consumedValue.Message != null)
                    {
                        _logger.LogInformation($"Consumed a message in {clientID} but not processed since either the key or value is null.");
                    }

                    await Task.Delay(1000);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Background service for {clientID} is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}