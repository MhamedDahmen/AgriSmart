using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Entities
{
    public abstract class Sensor
    {
        public string SensorId { get; protected set; }
        public string FieldId { get; protected set; }

        protected Sensor(string sensorId, string fieldId)
        {
            SensorId = sensorId;
            FieldId = fieldId;
        }

        public abstract double GenerateValue();
    }
}
