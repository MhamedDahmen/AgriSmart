using GpsTrackingSimulateur.Domain.Entities;
using GpsTrackingSimulateur.Domain.Interfaces;
using GpsTrackingSimulateur.Infrastructure.Mqtt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GpsTrackingSimulateur.Application.Services
{
    public class SimulationGpsService : IGpsSimulateur
    {
        private readonly CapteurGps _sensor;
        private readonly string _trackableId;
        private readonly string _fieldId;
        private readonly MqttPublisher _mqttPublisher;
        private bool _isRunning;

        public SimulationGpsService(string trackableId, string fieldId, double initialLat, double initialLng, MqttPublisher mqttPublisher)
        {
            _trackableId = trackableId;
            _fieldId = fieldId;
            _sensor = new CapteurGps(trackableId, fieldId, initialLat, initialLng);
            _mqttPublisher = mqttPublisher;
            _isRunning = true;
        }

        public async Task start()
        {
            new Thread(() =>
            {
                while (_isRunning)
                {
                    var (latitude, longitude) = _sensor.GenerateValue();
                    var payload = new
                    {
                        trackableId = _trackableId,
                        fieldId = _fieldId,
                        latitude = latitude,
                        longitude = longitude,
                        
                        timestamp = DateTime.UtcNow.ToString("o")
                    };

                    string json = JsonSerializer.Serialize(payload);
                    _mqttPublisher.Publish("tracking/equipment/position", json);

                    Console.WriteLine($"[GPS] {_trackableId} → Lat: {latitude}, Lng: {longitude}");
                    Thread.Sleep(5000);
                }
            }).Start();
        }

       
    }
}
