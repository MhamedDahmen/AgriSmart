using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Entities
{
    public class CapteurLumiere : Sensor
    {

        private static CapteurLumiere _instance;
        private static readonly object _lock = new();
        private Random _random;
        private double _lastValue;
        private CapteurLumiere(string sensorId, string fieldId) : base(sensorId, fieldId)
        {
            _random = new Random();
            _lastValue = _random.Next(300, 1000);
        }
        public static CapteurLumiere GetInstance(string sensorId, string fieldId)
        {
            lock (_lock)
            {
                _instance ??= new CapteurLumiere(sensorId, fieldId);
                return _instance;
            }
        }

        public override double GenerateValue()
        {
            var hour = DateTime.Now.Hour;
            double min = (hour >= 6 && hour <= 18) ? 300 : 0;
            double max = (hour >= 6 && hour <= 18) ? 1000 : 100;

            // Génère une variation douce autour de la dernière valeur
            double variation = _random.NextDouble() * 50 - 25; // entre -25 et +25
            _lastValue = Math.Clamp(_lastValue + variation, min, max);

            return Math.Round(_lastValue, 2);
        }

    }
}
