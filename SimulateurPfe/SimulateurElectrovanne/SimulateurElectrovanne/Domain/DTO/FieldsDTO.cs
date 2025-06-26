using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimulateurElectrovanne.Domain.DTO
{
    public class FieldsDTO
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("surface")]
        public double Surface { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonIgnore]
        public List<ElectrovanneDTO> Electrovannes { get; set; } = new();

    }
}
