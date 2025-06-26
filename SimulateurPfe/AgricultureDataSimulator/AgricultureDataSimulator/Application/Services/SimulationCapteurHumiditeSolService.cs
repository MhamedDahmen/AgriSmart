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
    public class SimulationCapteurHumiditeSolService : ICapteurSimulateur
    {
        private readonly CapteurHumiditeSol _Sensor;
        private readonly string _sensorId;
        private readonly string _fieldId;
        private readonly MqttPublisher _mqttPublisher;
        private bool _isRunning;

        public SimulationCapteurHumiditeSolService(string sensorId, string fieldId, MqttPublisher mqttPublisher)
        {
            _Sensor = CapteurHumiditeSol.GetInstance(sensorId, fieldId);
            _sensorId = sensorId;
            _fieldId = fieldId;
            _mqttPublisher = mqttPublisher;
            _isRunning = true;
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
                    _mqttPublisher.Publish("agriculture/Humidite/sol", json);

                    Console.WriteLine($"[Humidité sol] {_sensorId} → {value} %");
                    Thread.Sleep(5000);
                }
            }).Start();



        }
    }
}
