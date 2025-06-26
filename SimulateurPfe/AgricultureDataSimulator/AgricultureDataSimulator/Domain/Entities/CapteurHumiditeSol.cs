using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Entities
{
    public class CapteurHumiditeSol :Sensor
    {
        private static CapteurHumiditeSol _instance;
        private static readonly object _lock = new();
        private Random _random;
        private double _lastValue;

        private CapteurHumiditeSol(string sensorId, string fieldId) : base(sensorId, fieldId)
        {
            _random = new Random();
            _lastValue = _random.Next(40, 60);
        }
        public static CapteurHumiditeSol GetInstance(string sensorId, string fieldId)
        {
            lock (_lock)
            {
                _instance ??= new CapteurHumiditeSol(sensorId, fieldId);
                return _instance;
            }
        }


        public override double GenerateValue()
        {
            double variation = _random.NextDouble() * 4 - 2;
            _lastValue = Math.Clamp(_lastValue + variation, 10, 70);
            return Math.Round(_lastValue, 2);
        }

    }
}
