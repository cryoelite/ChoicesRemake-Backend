namespace KafkaService.Models
{
    public class KafkaConfiguration
    {
        public const string KafkaName = "Kafka";
        public string topic { get; set; }
        public string brokerURL { get; set; }
        public string group { get; set; }
        public string clientID { get; set; }

        public KafkaConfiguration(string ctortopic, string ctorbrokerURL, string ctorgroup, string clientName) => (topic, brokerURL, group, clientID) = (ctortopic, ctorbrokerURL, ctorgroup, clientName);
    }
}