using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.DTO
{
    public class BackUpData
    {
        [JsonPropertyName("users")]
        public List<UsersDTO> Users { get; set; }

        [JsonPropertyName("fields")]
        public List<FieldsDTO> Fields { get; set; }

        [JsonPropertyName("sensors")]
        public List<SensorsDTO> Sensors { get; set; }
    }
}
