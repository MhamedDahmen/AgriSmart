using AgricultureDataSimulator.Application.Services;
using AgricultureDataSimulator.Domain.DTO;
using AgricultureDataSimulator.Domain.Interfaces;
using AgricultureDataSimulator.Infrastructure.Mqtt;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Application
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

            var databackup = JsonSerializer.Deserialize<BackUpData>(json, options);
            if (databackup == null) return;

            foreach (var user in databackup.Users)
            {
                Console.WriteLine($"Simulation pour l'utilisateur: {user.Username}");

                foreach (var field in databackup.Fields)
                {
                        

                    Console.WriteLine($" Champ: {field.Name} ({field.Id})");

                    foreach (var sensor in databackup.Sensors)
                    {
                        if (sensor.FieldId != field.Id) continue;

                        Console.WriteLine($" Capteur: {sensor.Name} ({sensor.Type})");

                        switch (sensor.Type.ToLower())
                        {
                            case "lumiere":
                                var lumiereSim = new SimulationCapteurLumiereService(sensor.Id, field.Id, new MqttPublisher());
                                await lumiereSim.Start();
                                break;
                            case "temperature_sol":
                                var tempSim = new SimulationTemperatureSolService(sensor.Id, field.Id, new MqttPublisher());
                               await tempSim.Start();
                                break;
                            case "temperature_air":
                                var tempAirSim = new SimulationCapteurTemperatureAirService(sensor.Id, field.Id, new MqttPublisher());
                                await tempAirSim.Start();
                                break;
                            case "humidite_air":
                                var HumiditeAirSim = new SimulationCapteurHumiditeAirService(sensor.Id, field.Id, new MqttPublisher());
                                await HumiditeAirSim.Start();
                                break;
                            case "humidite_sol":
                                var HumiditesolSim = new SimulationCapteurHumiditeSolService(sensor.Id, field.Id, new MqttPublisher());
                                await HumiditesolSim.Start();
                                break;
                            case "ph_eau":
                                var PhEauSim = new SimulationCapteurPhEauService(sensor.Id, field.Id, new MqttPublisher());
                                await PhEauSim.Start();
                                break;
                            case "debit_eau":
                                var DebitEauSim = new SimulationCapteurDebitEauService(sensor.Id, field.Id, new MqttPublisher());
                                await DebitEauSim.Start();
                                break;
                            case "conductivite_eau":
                                var ConductiviteEauSim = new SimulationCapteurConductiviteEauService(sensor.Id, field.Id, new MqttPublisher());
                                await ConductiviteEauSim.Start();
                                break;
                            default:
                                Console.WriteLine(" Type de capteur non supporté");
                                break;
                        }
                    }
                }
            }
        }
    }
}
