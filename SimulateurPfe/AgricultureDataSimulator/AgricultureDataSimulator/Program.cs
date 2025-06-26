using AgricultureDataSimulator.Application;
using AgricultureDataSimulator.Application.Services;
using AgricultureDataSimulator.Domain.Interfaces;

class Program
{
    static async Task Main(string[] args)
    {
        IDatabaseConfig apiService = new DatabaseConfigService(); 
        var SimulationLoader = new SimulationLoader(apiService);

        await SimulationLoader.LoadStartSimulation();

        Console.WriteLine("Simulation terminée.");
    }
}
