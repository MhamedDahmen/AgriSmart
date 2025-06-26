using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Entities
{
    public class CapteurHumiditeAir : Sensor
    {
        private static CapteurHumiditeAir _instance;
        private static readonly object _lock = new();
        private Random _random;
        private double _lastValue;

        private CapteurHumiditeAir(string sensorId, string fieldId) : base(sensorId, fieldId)
        {
            _random = new Random();
            _lastValue = _random.Next(40, 60);
        }
        public static CapteurHumiditeAir GetInstance(string sensorId, string fieldId)
        {
            lock (_lock)
            {
                _instance ??= new CapteurHumiditeAir(sensorId, fieldId);
                return _instance;
            }
        }


        public override double GenerateValue()
        {
            double variation = _random.NextDouble() * 5 - 2.5;
            _lastValue = Math.Clamp(_lastValue + variation, 30, 90);
            return Math.Round(_lastValue, 2);
        }

    }
}
