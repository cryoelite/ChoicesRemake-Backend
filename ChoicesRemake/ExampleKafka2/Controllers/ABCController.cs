using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleKafka2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ABCController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var conf = new ConsumerConfig
            {
                GroupId = "foo",
                BootstrapServers = "broker:29092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId= Dns.GetHostName(),
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe("ExampleKafkaLayer");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };



                try
                {
                    var cr = c.Consume(cts.Token);
                    Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");

                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                }



                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                c.Close();

            }

            return new JsonResult("aa");

        }
    }
}
