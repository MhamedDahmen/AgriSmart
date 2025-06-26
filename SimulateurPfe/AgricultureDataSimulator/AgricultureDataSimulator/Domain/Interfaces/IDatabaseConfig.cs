using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgricultureDataSimulator.Domain.Interfaces
{
    public interface IDatabaseConfig
    {
        Task<string> GetBackupDataUrlAsync();
        Task<string> DownloadJsonFileAsync(string url);
    }
}
