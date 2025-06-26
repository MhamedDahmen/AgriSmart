using GpsTrackingSimulateur.Application.Services;
using GpsTrackingSimulateur.Domain.DTO;
using GpsTrackingSimulateur.Domain.Interfaces;
using GpsTrackingSimulateur.Infrastructure.Mqtt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GpsTrackingSimulateur.Application
{
    public class SimulationLoader
    {
        private readonly IDatabaseConfig _databaseConfig;

        public SimulationLoader(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public async Task LoadStartSimulation()
         {
            string jsonFileUrl = await _databaseConfig.GetBackupDataUrlAsync();
            string json = await _databaseConfig.DownloadJsonFileAsync(jsonFileUrl);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var databackup = JsonSerializer.Deserialize<BackupData>(json, options);
            if (databackup == null) return;
            foreach (var user in databackup.Users)
            {
                Console.WriteLine($"Simualtion pour l'utilisateur : {user.Username}");

                foreach (var field in databackup.Fields)
                {
                    if (field.UserId != user.Id) continue;
                    Console.WriteLine($"simulation pour le field : {field.Name}");
                    foreach (var trackable in databackup.Trackables)
                    {
                        if (trackable.FieldId != field.Id) continue;
                        Console.WriteLine($"Simulation pour le tracking : {trackable.Name}");

                        var GpsSim = new SimulationGpsService(trackable.Id, field.Id, field.Location.Latitude, field.Location.Longitude, new MqttPublisher () );
                        await GpsSim.start(); 
                    }
                   
                }


            }
        }
    }
}
