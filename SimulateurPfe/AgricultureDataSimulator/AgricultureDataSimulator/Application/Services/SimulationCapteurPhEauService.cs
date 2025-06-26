using AgricultureDataSimulator.Domain.Entities;
using AgricultureDataSimulator.Domain.Interfaces;
using AgricultureDataSimulator.Infrastructure.Mqtt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Application.Services
{
    public class SimulationCapteurPhEauService : ICapteurSimulateur
    {
        private readonly CapteurPhEau _Sensor;
        private readonly string _sensorId;
        private readonly string _fieldId;
        private readonly MqttPublisher _mqttPublisher;
        private bool _isRunning;

        public SimulationCapteurPhEauService (string sensorId, string fieldId ,MqttPublisher mqttPublisher)
        {
            _sensorId = sensorId;
            _fieldId = fieldId;
            _mqttPublisher = mqttPublisher;
            _isRunning = true;
           _Sensor = CapteurPhEau.GetInstance(sensorId, fieldId);

        }

        public async Task Start()
        {
            new Thread(() =>
            {
                while (_isRunning)
                {
                    var value = _Sensor.GenerateValue();
                    var payload = new
                    {
                        sensorId = _sensorId,
                        value = value,
                        fieldId = _fieldId,
                        timestamp = DateTime.UtcNow.ToString("o")
                    };

                    string json = System.Text.Json.JsonSerializer.Serialize(payload);
                    _mqttPublisher.Publish("agriculture/eau/ph", json);

                    Console.WriteLine($"[ph d'eau] {_sensorId} → {value} /14");
                    Thread.Sleep(5000);
                }
            }).Start();



        }
    }
}
