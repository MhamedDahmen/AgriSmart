using SimulateurElectrovanne.Application.Services;
using SimulateurElectrovanne.Domain.Interfaces;


class Program
{
    static async Task Main(string[] args)
    {
        IDatabaseConfig apiService = new DataBaseConfig();
        IMqttService mqttService = new MqttService();
        var SimulationLoader = new SimulationLoader(apiService,mqttService);

        await SimulationLoader.LoadStartSimulation();
        await Task.Delay(-1); // Keep app running
        //Console.WriteLine("Simulation terminée.");

        //var mqtt = new MqttService();
        //mqtt.Subscribe("test/topic", (message) =>
        //{
        //    Console.WriteLine($" Message reçu sur test/topic : {message}");
        //});

        //Console.WriteLine("Attente de messages...");
        //await Task.Delay(-1); // Keep app running

    }
}
