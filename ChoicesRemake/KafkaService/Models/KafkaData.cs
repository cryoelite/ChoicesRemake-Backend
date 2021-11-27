using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaService.Models
{
    public class KafkaData
    {
        public const string methodNameKey = "Method Name";
        public const string uidKey = "UID";
        public Headers headers;
        public KeyValuePair<string, string> message;

        public KafkaData(string invocationType, string methodName)
        {
            var guid = Guid.NewGuid();
            message = new KeyValuePair<string, string>(invocationType, ResultStatus.processing);
            headers = new Headers();
            //order of adding headers matter.
            AddHeader(uidKey, guid.ToString());
            AddHeader(methodNameKey, methodName);
        }

        public KafkaData(Headers headers, string invocationType, string resultStatus)
        {
            message = new KeyValuePair<string, string>(invocationType, resultStatus);
            this.headers = headers;
        }

        public void AddHeader(string key, string value)
        {
            var convValue = Encoding.UTF8.GetBytes(value);
            headers.Add(key, convValue);
        }

        public void AddRawHeader(string key, byte[] value)
        {
            headers.Add(key, value);
        }

        public Dictionary<string, string> GetAllHeaders()
        {
            var _headers = new Dictionary<string, string>();

            foreach (var elem in headers)
            {
                var value = elem.GetValueBytes();
                var stringData = Encoding.UTF8.GetString(value);
                _headers.Add(elem.Key, stringData);
            }
            return _headers;
        }

        public string? GetCustomHeader(string key)
        {
            var value = headers.GetLastBytes(key);
            if (value == null)
            {
                return null;
            }

            var convValue = Encoding.UTF8.GetString(value);
            return convValue;
        }

        public byte[]? GetCustomRawHeader(string key)
        {
            var value = headers.GetLastBytes(key);
            return value;
        }

        public string GetMethodName()
        {
            var uidValue = headers[1].GetValueBytes();
            var convValue = Encoding.UTF8.GetString(uidValue);
            return convValue;
        }

        public string GetUID()
        {
            var uidValue = headers[0].GetValueBytes();
            var convValue = Encoding.UTF8.GetString(uidValue);
            return convValue;
        }

        public void MarkError() => message = new KeyValuePair<string, string>(message.Key, ResultStatus.unavailable);

        public void MarkFailure() => message = new KeyValuePair<string, string>(message.Key, ResultStatus.failure);

        public void MarkSuccess() => message = new KeyValuePair<string, string>(message.Key, ResultStatus.success);

        public void RemoveHeader(string key)
        {
            headers.Remove(key);
        }
    }
}