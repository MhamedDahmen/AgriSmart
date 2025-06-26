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
    public class SimulationCapteurLumiereService : ICapteurSimulateur
    {
        private readonly CapteurLumiere _Sensor;
        private readonly string _sensorId;
        private readonly string _fieldId;
        private readonly MqttPublisher _mqttPublisher;
        private bool _isRunning;

        public SimulationCapteurLumiereService(string sensorId, string fieldId, MqttPublisher mqttPublisher)
        {
            _sensorId = sensorId;
            _fieldId = fieldId;
            _Sensor = CapteurLumiere.GetInstance(sensorId,fieldId);
            _mqttPublisher = new MqttPublisher();
            _isRunning = true;
        }

        public async Task Start() {
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
                    _mqttPublisher.Publish("agriculture/lumiere", json);

                    Console.WriteLine($"[Lumière] {_sensorId} → {value} lux");
                    Thread.Sleep(5000); // 5s entre chaque envoi
                }
            }).Start();



        }

    }
}
