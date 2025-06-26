using GpsTrackingSimulateur.Domain.Interfaces;
using GpsTrackingSimulateur.Application.Services;
using GpsTrackingSimulateur.Application;

class Program
{
    static async Task Main(string[] args)
    {
        IDatabaseConfig apiService = new DataBaseConfig();
        var SimulationLoader = new SimulationLoader(apiService);

        await SimulationLoader.LoadStartSimulation();

        Console.WriteLine("Simulation terminée.");
    }
}
