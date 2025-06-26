using GpsTrackingSimulateur.Domain.DTO;
using GpsTrackingSimulateur.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsTrackingSimulateur.Application.Services
{
    public class DataBaseConfig : IDatabaseConfig 
    {
        private readonly HttpClient _httpClient;

        public DataBaseConfig()
        {
            _httpClient = new HttpClient();
        }
        public async Task<string> GetBackupDataUrlAsync()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:5000/api/backup/latest-backup");
            var jsonResponse = JsonConvert.DeserializeObject<BackupResponse>(response);
            return jsonResponse?.Url;
        }
        public async Task<string> DownloadJsonFileAsync(string url)
        {
            var response = await _httpClient.GetStringAsync(url);

            return response;
        }
    }
}
