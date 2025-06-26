//using MQTTnet;
//using MQTTnet.Client;
//using MQTTnet.Client.Options;
//using SimulateurElectrovanne.Domain.Interfaces;
//using System;
//using System.Text;
//using System.Threading.Tasks;

//namespace SimulateurElectrovanne.Application.Services
//{
//    public class MqttService : IMqttService
//    {
//        private readonly IMqttClient _client;

//        public MqttService()
//        {
//            var factory = new MqttFactory();
//            this._client = factory.CreateMqttClient();

//            var options = new MqttClientOptionsBuilder()
//                .WithTcpServer("localhost", 1883)
//                .Build();

//            _client.ConnectAsync(options).Wait();
//        }

//        public void Subscribe(string topic, Action<string> callback)
//        {
//            _client.UseApplicationMessageReceivedHandler(e =>
//            {
//                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
//                callback(message);
//            });

//            _client.SubscribeAsync(topic).Wait();
//            Console.WriteLine($"✅ Subscribed to topic: {topic}");
//        }
//    }
//}


//using MQTTnet;
//using MQTTnet.Client;
//using MQTTnet.Client.Options;
//using SimulateurElectrovanne.Domain.Interfaces;
//using System;
//using System.Collections.Concurrent;
//using System.Text;
//using System.Threading.Tasks;

//namespace SimulateurElectrovanne.Application.Services
//{
//    public class MqttService : IMqttService
//    {
//        private readonly IMqttClient _client;
//        private readonly ConcurrentDictionary<string, Action<string>> _callbacks = new();

//        public MqttService()
//        {
//            var factory = new MqttFactory();
//            _client = factory.CreateMqttClient();

//            var options = new MqttClientOptionsBuilder()
//                .WithTcpServer("localhost", 1883)
//                .Build();

//            _client.UseApplicationMessageReceivedHandler(e =>
//            {
//                var topic = e.ApplicationMessage.Topic;
//                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

//                if (_callbacks.TryGetValue(topic, out var callback))
//                {
//                    callback(message);
//                }
//                else
//                {
//                    Console.WriteLine($"⚠️ Message reçu sur un topic non géré : {topic}");
//                }
//            });

//            _client.ConnectAsync(options).Wait();
//        }

//        public void Subscribe(string topic, Action<string> callback)
//        {
//            if (!_callbacks.ContainsKey(topic))
//            {
//                _callbacks[topic] = callback;
//                _client.SubscribeAsync(topic).Wait();
//                Console.WriteLine($"✅ Subscribed to topic: {topic}");
//            }
//        }
//    }
//}



using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet;
using SimulateurElectrovanne.Domain.Interfaces;
using System.Text;

public class MqttService : IMqttService
{
    private readonly IMqttClient _client;
    private readonly Dictionary<string, Action<string>> _topicHandlers = new();

    public MqttService()
    {
        var factory = new MqttFactory();
        this._client = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .Build();

        _client.UseApplicationMessageReceivedHandler(e =>
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? Array.Empty<byte>());
            Console.WriteLine($" Reçu → Topic: {topic} | Message: {payload}");

            if (_topicHandlers.TryGetValue(topic, out var handler))
            {
                handler(payload);
            }
            else
            {
                Console.WriteLine($" Message reçu sur un topic non géré: {topic}");
            }
        });

        _client.ConnectAsync(options).Wait();
        Console.WriteLine(" Connecté au broker MQTT !");
    }

    public void Subscribe(string topic, Action<string> callback)
    {
        if (!_topicHandlers.ContainsKey(topic))
        {
            _topicHandlers[topic] = callback;
            _client.SubscribeAsync(topic).Wait();
            Console.WriteLine($"Subscribed to topic: {topic}");
        }
    }
}
