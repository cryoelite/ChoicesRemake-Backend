using System;

namespace KafkaMessageMarshaller
{
    public class KafkaMessageService
    {
        public const string delimiter = "[%X509]";
        public string Serializer(string token, string role)
        {
            return token + delimiter + role;
        }
        public string Deserializer(string data)
        {
            var parts = data.Split(delimiter);
        }
    }
}
