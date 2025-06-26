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
    public class SimulationCapteurHumiditeAirService : ICapteurSimulateur
    {
        private readonly CapteurHumiditeAir _Sensor;
        private readonly string _sensorId;
        private readonly string _fieldId;
        private readonly MqttPublisher _mqttPublisher;
        private bool _isRunning;

        public SimulationCapteurHumiditeAirService( string sensorId, string fieldId, MqttPublisher mqttPublisher)
        {
            _Sensor = CapteurHumiditeAir.GetInstance(sensorId, fieldId);
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
                    _mqttPublisher.Publish("agriculture/Humidite/air", json);

                    Console.WriteLine($"[Humidité air] {_sensorId} → {value} %");
                    Thread.Sleep(5000); 
                }
            }).Start();



        }
    }
}
