using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Entities
{
    public class CapteurPhEau :Sensor
    {
        private static CapteurPhEau _instance;
        private static readonly object _lock = new();
        private Random _random;
        private double _lastValue;
        private CapteurPhEau(string sensorId, string fieldId) : base(sensorId, fieldId)
        {
            _random = new Random();
            _lastValue = 7.0; 
        }
        public static CapteurPhEau GetInstance(string sensorId, string fieldId)
        {
            lock (_lock)
            {
                _instance ??= new CapteurPhEau(sensorId, fieldId);
                return _instance;
            }
        }

        public override double GenerateValue()
        {
            double variation = _random.NextDouble() * 0.3 - 0.15;
            _lastValue = Math.Clamp(_lastValue + variation, 5.5, 8.5);
            return Math.Round(_lastValue, 2);
        }

    }
}
