using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GpsTrackingSimulateur.Domain.DTO
{
    public class BackupData
    {
        [JsonPropertyName("users")]
        public List<UsersDTO> Users { get; set; }

        [JsonPropertyName("fields")]
        public List<FieldsDTO> Fields { get; set; }

        [JsonPropertyName("trackables")]
        public List<TrackablesDTO> Trackables { get; set; }
    }
}
