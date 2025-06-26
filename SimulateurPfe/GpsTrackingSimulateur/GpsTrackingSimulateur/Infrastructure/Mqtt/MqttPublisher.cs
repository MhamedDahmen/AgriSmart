using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace GpsTrackingSimulateur.Infrastructure.Mqtt
{
    public class MqttPublisher
    {
        private readonly IMqttClient _client;
        private readonly IMqttClientOptions _options;
        public MqttPublisher()
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            _client.ConnectAsync(_options, CancellationToken.None).Wait();
        }

        public void Publish(string topic, string message)
        {
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(Encoding.UTF8.GetBytes(message))
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            _client.PublishAsync(mqttMessage, CancellationToken.None).Wait();
        }
    }
}
