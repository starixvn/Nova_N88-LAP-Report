using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fanex.Athena.Report.Common.Extensions;
using Fanex.Athena.Report.SabaWinLossAnalysis.DTOs;
using Fanex.Athena.Report.SabaWinLossAnalysis.MetaModels;
using Fanex.Athena.Report.SabaWinLossAnalysis.Repositories.Interfaces;
using Fanex.Data;

namespace Fanex.Athena.Report.SabaWinLossAnalysis.Repositories
{
    public class WinlossAnalysisRepository : IWinlossAnalysisRepository
    {
        public Task<WinlossAnalysisAggregateDTO> GetDataByAgencyCriteria(WinlossAnalysisMetaModel metaModel)
        {
            const string spName = "LAP_Rpt_WinlossAnalysis_AgencySel";
            using (var objectDb = ObjectDbFactory.CreateInstance(spName))
            {
                AddCommonParameter(objectDb, metaModel);
                return ReadDataAsync(objectDb);
            }
        }

        public Task<WinlossAnalysisAggregateDTO> GetDataByPlayerCriteria(WinlossAnalysisMetaModel metaModel)
        {
            const string spName = "LAP_Rpt_WinlossAnalysis_PlayerSel";
            using (var objectDb = ObjectDbFactory.CreateInstance(spName))
            {
                AddCommonParameter(objectDb, metaModel);
                objectDb.SetParameterValue("IsVIPOnly", metaModel.IsVipOnly);
                objectDb.SetParameterValue("CustomerClass", metaModel.CustomerClass);
                objectDb.SetParameterValue("FullCustomerClass", metaModel.FullCustomerClass);
                objectDb.SetParameterValue("OddsGroup", metaModel.OddsGroup);
                objectDb.SetParameterValue("SearchMode", metaModel.SearchMode);
                if (!string.IsNullOrEmpty(metaModel.SearchUserName))
                {
                    objectDb.SetParameterValue("SearchUserName", metaModel.SearchUserName);
                }
                return ReadDataAsync(objectDb);
            }
        }

        private static void AddCommonParameter(IObjectDb objectDb, WinlossAnalysisMetaModel metaModel)
        {
            objectDb.SetParameterValue("FromDate", metaModel.FromDate);
            objectDb.SetParameterValue("ToDate", metaModel.ToDate);
            objectDb.SetParameterValue("MovebetDay", metaModel.MovebetDay);
            objectDb.SetParameterValue("AgentList", metaModel.AgentListXml);
            objectDb.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            objectDb.SetParameterValue("ProductCodes", metaModel.ProductCodes);
            objectDb.SetParameterValue("SportIds", metaModel.SportIds);
            objectDb.SetParameterValue("BettypeIds", metaModel.BetTypeIds);
            objectDb.SetParameterValue("StatusIds", metaModel.StatusIds);
            objectDb.SetParameterValue("PlatformIds", metaModel.PlatformIds);
            objectDb.SetParameterValue("LiveStatusIds", metaModel.LiveStatusIds);
            objectDb.SetParameterValue("CashOutMode", metaModel.CashOutMode);
            objectDb.SetParameterValue("GroupMode", metaModel.GroupMode);
            objectDb.SetParameterValue("BreakdownMode", metaModel.BreakdownMode);
            objectDb.SetParameterValue("PageIndex", metaModel.PageIndex);
            objectDb.SetParameterValue("PageSize", metaModel.PageSize);
            objectDb.SetParameterValue("SortKey", metaModel.SortBy);
            objectDb.SetParameterValue("SortType", metaModel.SortOrder);
            objectDb.SetParameterValue("SpecialPlatform", metaModel.SpecialPlatformType);
        }

        private static async Task<WinlossAnalysisAggregateDTO> ReadDataAsync(IObjectDb objectDb)
        {
            var result = new WinlossAnalysisAggregateDTO();
            using (var reader = await objectDb.ExecuteReaderAsync())
            {
                result.MainData = ReadMainDataAsync(reader);
                reader.NextResult();
                result.GroupSumData = ReadGroupSumDataAsync(reader);
                reader.NextResult();
                result.SumTotal = ReadSumTotalAsync(reader);
            }
            return result;
        }

        private static IEnumerable<WinlossAnalysisDTO> ReadMainDataAsync(IDataReader reader)
        {
            var listMainData = new List<WinlossAnalysisDTO>();
            while (reader.Read())
            {
                var data = new WinlossAnalysisDTO
                {
                    RowIndex = reader.GetVal<int>("ridx"),
                    GroupBy = reader.GetVal<string>("GroupBy"),
                    BreakBy = reader.GetVal<string>("BreakBy"),
                };
                ReadCommonField(reader, data);
                listMainData.Add(data);
            }
            return listMainData;
        }

        private static IEnumerable<WinlossAnalysisDTO> ReadGroupSumDataAsync(IDataReader reader)
        {
            var listGroupSumData = new List<WinlossAnalysisDTO>();
            while (reader.Read())
            {
                var data = new WinlossAnalysisDTO
                {
                    RowIndex = reader.GetVal<int>("ridx"),
                    GroupBy = reader.GetVal<string>("GroupBy"),
                };
                ReadCommonField(reader, data);
                listGroupSumData.Add(data);
            }

            return listGroupSumData;
        }

        private static SumTotalWinlossAnalysisDTO ReadSumTotalAsync(IDataReader reader)
        {
            reader.Read();
            var sumData = new SumTotalWinlossAnalysisDTO
            {
                TotalRow = reader.GetVal<int>("TotalRow")
            };
            ReadCommonField(reader, sumData);

            return sumData;
        }

        private static void ReadCommonField(IDataReader reader, WinlossAnalysisDTO data)
        {
            data.BetCount = reader.GetVal<int>("BetCount");
            data.Turnover = reader.GetVal<decimal>("Turnover");
            data.NetTurnover = reader.GetVal<decimal>("NetTurnover");
            data.MemWinloss = reader.GetVal<decimal>("MemWinloss");
            data.MemComm = reader.GetVal<decimal>("MemComm");
            data.LicWinloss = reader.GetVal<decimal>("LicWinloss");
            data.LicComm = reader.GetVal<decimal>("LicComm");
            data.AmtDueToBA = reader.GetVal<decimal>("AmtDueToBA");
            data.BetEffective = reader.GetVal<decimal>("BetEffective");
            data.Margin = reader.GetVal<float>("Margin");
            data.BaseTurnover = reader.GetVal<decimal>("BaseTurnover");
            data.BaseNetTurnover = reader.GetVal<decimal>("BaseNetTurnover");
            data.BaseMemWinloss = reader.GetVal<decimal>("BaseMemWinloss");
            data.BaseMemComm = reader.GetVal<decimal>("BaseMemComm");
            data.BaseLicWinloss = reader.GetVal<decimal>("BaseLicWinloss");
            data.BaseLicComm = reader.GetVal<decimal>("BaseLicComm");
            data.BaseAmtDueToBA = reader.GetVal<decimal>("BaseAmtDueToBA");
            data.BaseBetEffective = reader.GetVal<decimal>("BaseBetEffective");
        }
    }
}