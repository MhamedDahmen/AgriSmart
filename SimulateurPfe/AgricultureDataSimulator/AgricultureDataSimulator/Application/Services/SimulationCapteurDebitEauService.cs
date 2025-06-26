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
    public class SimulationCapteurDebitEauService : ICapteurSimulateur
    {
        private readonly CapteurDebitEau _Sensor;
        private readonly string _sensorId;
        private readonly string _fieldId;
        private readonly MqttPublisher _mqttPublisher;
        private bool _isRunning;

        public SimulationCapteurDebitEauService(string sensorId, string fieldId, MqttPublisher mqttPublisher)
        {
            _sensorId = sensorId;
            _fieldId = fieldId;
            _mqttPublisher = mqttPublisher;
            _isRunning = true;
            _Sensor= CapteurDebitEau.GetInstance(sensorId, fieldId);

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
                    _mqttPublisher.Publish("agriculture/eau/debit", json);

                    Console.WriteLine($"[débit d'eau] {_sensorId} → {value} L/min");
                    Thread.Sleep(5000);
                }
            }).Start();



        }
    }
}
