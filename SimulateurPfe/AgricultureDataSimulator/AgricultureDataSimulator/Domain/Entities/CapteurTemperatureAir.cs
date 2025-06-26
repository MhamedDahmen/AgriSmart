﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Entities
{
    public class CapteurTemperatureAir : Sensor
    {
        private static CapteurTemperatureAir _instance;
        private static readonly object _lock = new();
        private Random _random;

        private CapteurTemperatureAir(string sensorId, string fieldId) : base(sensorId, fieldId)
        {
            _random = new Random();
        }

        public static CapteurTemperatureAir GetInstance(string sensorId, string fieldId)
        {
            lock (_lock)
            {
                _instance ??= new CapteurTemperatureAir(sensorId, fieldId);
                return _instance;
            }
        }

        public override double GenerateValue()
        {

            double heure = DateTime.Now.Hour;
            double baseValue = heure switch
            {
                >= 6 and < 12 => 15 + _random.NextDouble() * 3,
                >= 12 and < 18 => 18 + _random.NextDouble() * 3,
                >= 18 and < 22 => 14 + _random.NextDouble() * 2,
                _ => 10 + _random.NextDouble() * 2
            };

            return Math.Round(baseValue, 2);
        }
    }
}
