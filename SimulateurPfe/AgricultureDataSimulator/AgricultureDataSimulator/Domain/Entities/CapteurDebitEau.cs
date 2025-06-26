using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Entities
{
    public class CapteurDebitEau : Sensor 
    {
        private static CapteurDebitEau _instance;
        private static readonly object _lock = new();
        private Random _random;
        private double _lastValue;
        private CapteurDebitEau(string sensorId, string fieldId) : base(sensorId, fieldId)
        {
            _random = new Random();
            _lastValue = _random.NextDouble() * 5 + 1; // 1 - 6 L/min
        }

        public static CapteurDebitEau GetInstance(string sensorId, string fieldId)
        {
            lock (_lock)
            {
                _instance ??= new CapteurDebitEau(sensorId, fieldId);
                return _instance;
            }
        }

        public override double GenerateValue()
        {
            double variation = _random.NextDouble() * 0.5 - 0.25;
            _lastValue = Math.Clamp(_lastValue + variation, 0.5, 10);
            return Math.Round(_lastValue, 2);
        }
    }
}
