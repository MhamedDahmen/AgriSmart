using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Entities
{
    public class CapteurConductiviteEau : Sensor
    {
        private static CapteurConductiviteEau _instance;
        private static readonly object _lock = new();
        private Random _random;
        private double _lastValue;

        private CapteurConductiviteEau(string sensorId, string fieldId) : base(sensorId, fieldId)
        {
            _random = new Random();
            _lastValue = _random.Next(300, 700);
        }

        public static CapteurConductiviteEau GetInstance(string sensorId, string fieldId)
        {
            lock (_lock)
            {
                _instance ??= new CapteurConductiviteEau(sensorId, fieldId);
                return _instance;
            }
        }

        public override double GenerateValue()
        {
            double variation = _random.NextDouble() * 50 - 25;
            _lastValue = Math.Clamp(_lastValue + variation, 100, 2000);
            return Math.Round(_lastValue, 2);
        }
    }

}
