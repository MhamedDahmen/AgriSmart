using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GpsTrackingSimulateur.Domain.DTO
{
    public class UsersDTO
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
