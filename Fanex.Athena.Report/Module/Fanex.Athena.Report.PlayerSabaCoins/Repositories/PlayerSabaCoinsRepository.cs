using Fanex.Athena.Report.PlayerSabaCoins.Consts;
using Fanex.Athena.Report.PlayerSabaCoins.DTO;
using Fanex.Athena.Report.PlayerSabaCoins.MetaModels;
using Fanex.Athena.Report.PlayerSabaCoins.Repositories.Interfaces;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.PlayerSabaCoins.Repositories
{
    public class PlayerSabaCoinsRepository : IPlayerSabaCoinsRepository
    {
        private readonly IBalanceUpService _balanceUpService;

        public PlayerSabaCoinsRepository(IBalanceUpService balanceUpService)
        {
            _balanceUpService = balanceUpService;
        }

        public async Task<IEnumerable<PlayerSabaCoinsDTO>> GetMainData(PlayerSabaCoinsMetaModel metaModel)
        {
            using (var db = ObjectDbFactory.CreateInstance(SpConsts.PlayerSabaCoinsMain))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("fromdate", metaModel.FromDate.Date);
                db.SetParameterValue("todate", metaModel.ToDate.Date);
                db.SetParameterValue("Username", metaModel.Username);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    return ReadMainData(reader);
                }
            }
        }

        private static IEnumerable<PlayerSabaCoinsDTO> ReadMainData(IDataReader reader)
        {
            var dataList = new List<PlayerSabaCoinsDTO>();

            while (reader.Read())
            {
                var item = new PlayerSabaCoinsDTO
                {
                    CustId = reader["custId"].ToString(),
                    Username = reader["username"].ToString(),
                    ExternalId = reader["nickname"].ToString(),
                    OpeningBalance = Convert.ToDecimal(reader["OpeningBalance"]),
                    Turnover = Convert.ToDecimal(reader["Turnover"]),
                    Outstanding = Convert.ToDecimal(reader["Outstanding"]),
                    WinLoss = Convert.ToDecimal(reader["WinLoss"]),
                    ClosingBalance = Convert.ToDecimal(reader["ClosingBalance"])
                };

                dataList.Add(item);
            }

            return dataList;
        }

        public async Task<IEnumerable<PlayerSabaCoinsDTO>> GetExtraInfoData(PlayerSabaCoinsMetaModel metaModel)
        {
            var balanceUpInfo = await _balanceUpService.GetBalanceUpAsync();

            using (var db = ObjectDbFactory.CreateInstance(SpConsts.PlayerSabaCoinsExtraInfo))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("fromdate", metaModel.FromDate.Date);
                db.SetParameterValue("todate", metaModel.ToDate.Date);
                db.SetParameterValue("Username", metaModel.Username);
                db.SetParameterValue("movebetsday", balanceUpInfo.MoveBetsDay);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    return ReadExtraInfoData(reader);
                }
            }
        }

        private static IEnumerable<PlayerSabaCoinsDTO> ReadExtraInfoData(IDataReader reader)
        {
            var dataList = new List<PlayerSabaCoinsDTO>();

            while (reader.Read())
            {
                var item = new PlayerSabaCoinsDTO
                {
                    CustId = reader["custId"].ToString(),
                    RewardPromotionAmt = Convert.ToDecimal(reader["RewardPromotionAmt"]),
                    ExchangeAmt = Convert.ToDecimal(reader["ExchangeAmt"])
                };

                dataList.Add(item);
            }

            return dataList;
        }
    }
}