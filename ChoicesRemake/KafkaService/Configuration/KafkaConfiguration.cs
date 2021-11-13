namespace KafkaService.Models
{
    public class KafkaConfiguration
    {
        public KafkaConfiguration(string ctortopicPrimary, string ctorbrokerURL, string ctorgroupPrimary, string clientName, string? ctortopicSecondary = null, string? ctorgroupSecondary = null) => (topic_primary, brokerURL, group_primary, clientID, topic_secondary, group_secondary) = (ctortopicPrimary, ctorbrokerURL, ctorgroupPrimary, clientName, ctortopicSecondary, ctorgroupSecondary);

        public string brokerURL { get; set; }
        public string clientID { get; set; }
        public string group_primary { get; set; }
        public string? group_secondary { get; set; }
        public string topic_primary { get; set; }
        public string? topic_secondary { get; set; }
    }
}