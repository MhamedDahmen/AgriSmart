using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimulateurElectrovanne.Domain.DTO
{
    public class BackupData
    {
        [JsonPropertyName("users")]
        public List<UsersDTO> Users { get; set; }

        [JsonPropertyName("fields")]
        public List<FieldsDTO> Fields { get; set; }

        [JsonPropertyName("electrovannes")]
        public List<ElectrovanneDTO> Electrovannes { get; set; }

    }
}
