
using SimulateurElectrovanne.Domain.DTO;
using SimulateurElectrovanne.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimulateurElectrovanne.Application.Services
{
    public class SimulationLoader
    {
        private readonly IDatabaseConfig _databaseConfig;
        private readonly IMqttService _mqttService;

        public SimulationLoader(IDatabaseConfig databaseConfig, IMqttService mqttService)
        {
            _databaseConfig = databaseConfig;
            _mqttService = mqttService;
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
                    foreach (var vanne in databackup.Electrovannes)
                    {
                        if (vanne.FieldId != field.Id) continue;

                        Console.WriteLine($"simulation pour l'electrovanne : {vanne.Name}");

                        _mqttService.Subscribe(vanne.Topic, (message) =>
                        {
                            try
                            {
                                var payload = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
                                if (payload != null && payload.TryGetValue("status", out var status))
                                {
                                    if (status.ToLower() == "on")
                                        Console.WriteLine($"💧 Vanne '{vanne.Name}' est en fonctionnement !");
                                    else if (status.ToLower() == "off")
                                        Console.WriteLine($"🚫 Vanne '{vanne.Name}' est fermée.");
                                    else
                                        Console.WriteLine($"❓ Vanne '{vanne.Name}' → Status inconnu : {status}");
                                }
                                else
                                {
                                    Console.WriteLine($"⚠️ Vanne '{vanne.Name}' → Message mal formé : {message}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"❌ Erreur de traitement du message pour la vanne '{vanne.Name}' : {ex.Message}");
                            }
                        });


                    }

                }


            }

        }
    }
}
