using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulateurElectrovanne.Domain.Interfaces
{
    public interface IMqttService
    {
        void Subscribe(string topic, Action<string> callback);
    }
}
