using AgricultureDataSimulator.Domain.Entities;
using AgricultureDataSimulator.Domain.Interfaces;
using AgricultureDataSimulator.Infrastructure.Mqtt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Application.Services
{
    public class SimulationCapteurTemperatureAirService : ICapteurSimulateur
    {
        private readonly CapteurTemperatureAir _sensor;
        private readonly string _sensorId;
        private readonly string _fieldId;
        private readonly MqttPublisher _mqttPublisher;
        private bool _isRunning;

        public SimulationCapteurTemperatureAirService(string sensorId, string fieldId, MqttPublisher mqttPublisher)
        {
            _sensorId = sensorId;
            _fieldId = fieldId;
            _sensor = CapteurTemperatureAir.GetInstance(sensorId, fieldId);
            _mqttPublisher = new MqttPublisher();
            _isRunning = true;
        }

        public async Task Start()
        {
            new Thread(() =>
            {
                while (_isRunning)
                {
                    var value = _sensor.GenerateValue();
                    var payload = new
                    {
                        sensorId = _sensorId,
                        value = value,
                        fieldId = _fieldId,
                        timestamp = DateTime.UtcNow.ToString("o")
                    };

                    string json = JsonSerializer.Serialize(payload);
                    _mqttPublisher.Publish("agriculture/Temperature/air", json);

                    Console.WriteLine($"[Temp Air] {_sensorId} → {value} °C");
                    Thread.Sleep(5000);
                }
            }).Start();
        }
    }
}
