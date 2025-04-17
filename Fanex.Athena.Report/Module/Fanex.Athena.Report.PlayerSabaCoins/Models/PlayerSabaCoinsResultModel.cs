using Fanex.Athena.Report.PlayerSabaCoins.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.Report.PlayerSabaCoins.Models
{
    public class PlayerSabaCoinsResultModel
    {
        public List<PlayerSabaCoinsDTO> Data { get; set; }
        public PlayerSabaCoinsDTO TotalSummary { get; set; }
        public int TotalRows { get; set; }
    }
}