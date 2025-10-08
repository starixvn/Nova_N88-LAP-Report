using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Extensions;
using Fanex.Athena.Report.Common.Model.DataDomain;
using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Model.ResultModel;
using Fanex.Athena.Report.Common.Model.ViewModel;

using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Core.Helpers;
using Fanex.Athena.Report.ExtraPayout.Const;
using Fanex.Athena.Report.ExtraPayout.DTO;
using Fanex.Athena.Report.ExtraPayout.MetaModels;
using Fanex.Athena.Report.ExtraPayout.Repositories.Interfaces;
using Fanex.Athena.Report.Resources;
using Fanex.Athena.Report.Resources.Constants;
using Fanex.Athena.Report.Resources.Helpers;
using Fanex.Data;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using System.Threading.Tasks;

namespace Fanex.Athena.Report.ExtraPayout.Repositories
{
    public class ExtraPayoutRepository : IExtraPayoutRepository
    {
        private readonly IBalanceUpService _balanceUpService;
        private readonly ICacheRefAdapter _cacheRefAdapter;
        private readonly ICurrencyService _currencyService;

        private static readonly IDictionary<int, int> _extraPayoutMapType = new Dictionary<int, int>
        {
            { ExtraPayoutMapType.PPTournamentPrize, BetType.PPTournamentPrize },
            { ExtraPayoutMapType.PPPromotionPrize, BetType.PPPromotionPrize },
            { ExtraPayoutMapType.PPJackpotPrize, BetType.PPJackpotPrize },
            { ExtraPayoutMapType.PPJackpotContribution, BetType.PPJackpotContribution },
            { ExtraPayoutMapType.SabaLuckySpin, BetType.SabaLuckySpin },
            { ExtraPayoutMapType.SabaLuckySpot, BetType.SabaLuckySpot },
            { ExtraPayoutMapType.SabaNonCashPrize, BetType.SabaNonCashPrize },
            { ExtraPayoutMapType.SabaLoyaltyPointsExchange, BetType.SabaLoyaltyPointsExchange },
            { ExtraPayoutMapType.SabaRedEnvelope, BetType.SabaRedEnvelope },
            { ExtraPayoutMapType.SabaCoinsCashExchange, BetType.SabaCoinsCashExchange },
            { ExtraPayoutMapType.PromotionJackpotPrize, BetType.PromotionJackpotPrize },
            { ExtraPayoutMapType.PromotionCashPrize, BetType.PromotionCashPrize },
            { ExtraPayoutMapType.SAGamingTip, BetType.SAGamingTip },
            { ExtraPayoutMapType.SCBFreeBets, BetType.SCBFreeBets },
            { ExtraPayoutMapType.SCBFreeBetsJackpotPrize, BetType.SCBFreeBetsJackpotPrize },
            { ExtraPayoutMapType.SCBFreeBetsJackpotContribution, BetType.SCBFreeBetsJackpotContribution },
            { ExtraPayoutMapType.EASexyLuckyDraw, BetType.EASexyLuckyDraw },
            { ExtraPayoutMapType.ArcadiaGamingBonus, BetType.ArcadiaGamingBonus },
            { ExtraPayoutMapType.VgamingFreeBets, BetType.VgamingFreeBets },
            { ExtraPayoutMapType.VgamingPromotionPrize, BetType.VgamingPromotionPrize },
            { ExtraPayoutMapType.VgamingFreeBetsJackpotPrize, BetType.VgamingFreeBetsJackpotPrize },
            { ExtraPayoutMapType.VgamingFreeBetsJackpotContribution, BetType.VgamingFreeBetsJackpotContribution },
            { ExtraPayoutMapType.PayoutStreamerTips, BetType.TIPStreamerTips },
            { ExtraPayoutMapType.PayoutSabaCasinoRoyaleTournamentPrize, BetType.SabaCasinoRoyaleTournamentPrize },
            { ExtraPayoutMapType.PayoutSabaRewardRiskFreeRebate, BetType.SabaRewardRiskFreeRebate },
            { ExtraPayoutMapType.PayoutOddsBoostAdjustment, BetType.OddsBoostAdjustment },
            { ExtraPayoutMapType.PayoutNumberGameLuckyDrawPromotion, BetType.NumberGameLuckyDrawPromotion },
            { ExtraPayoutMapType.PayoutNBAPlayoffsLotteryPromotion, BetType.NBAPlayoffsLotteryPromotion },
            { ExtraPayoutMapType.PayoutNBABalanceAdjustment, BetType.NBABalanceAdjustment },
            { ExtraPayoutMapType.PayoutFIFACWCScoreChallenge, BetType.FIFACWCScoreChallenge }
        };

        private static readonly int[] SBPromotionBettypes = new[] {
            BetType.SabaLuckySpin,
            BetType.SabaLuckySpot,
            BetType.SabaNonCashPrize,
            BetType.SabaLoyaltyPointsExchange,
            BetType.SabaRedEnvelope,
            BetType.SabaRewardRiskFreeRebate,
            BetType.SabaCoinsCashExchange,
            BetType.PromotionJackpotPrize,
            BetType.PromotionCashPrize,
            BetType.NBAPlayoffsLotteryPromotion,
            BetType.NBABalanceAdjustment,
            BetType.FIFACWCScoreChallenge
        };

        private static readonly int[] CasinoPromotionBettypes = new[] {
            BetType.SabaCasinoRoyaleTournamentPrize
        };

        private static readonly int[] NumberGamePromotionBettypes = new[] {
            BetType.NumberGameLuckyDrawPromotion
        };

        public ExtraPayoutRepository(
            IBalanceUpService balanceUpService, ICacheRefAdapter cacheRefAdapter, ICurrencyService currencyService)
        {
            _balanceUpService = balanceUpService;
            _cacheRefAdapter = cacheRefAdapter;
            _currencyService = currencyService;
        }

        public async Task<SortableResultModel<ExtraPayoutDTO>> GetExtraPayoutMainData(
            ExtraPayoutMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_ExtraPayout";
            var balanceUpInfo = await _balanceUpService.GetBalanceUpAsync().ConfigureAwait(false);

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue("ReportType", metaModel.ReportBy);
                db.SetParameterValue("Username", metaModel.Username);
                UpdateSpCommonParamsAsync(db, metaModel, balanceUpInfo);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return await GetMainDataAsync(reader, metaModel);
                }
            }
        }

        private static void ReadCommonData(IDataReader reader, BaseExtraPayoutDTO data, ExtraPayoutMetaModel metaModel, IEnumerable<Currency> currencyList)
        {
            var productCode = reader.GetVal<string>("ProductCode") ?? string.Empty;
            var currency = reader.GetVal<int>("Currency");
            var basedSharingAmount = reader.GetVal<decimal>("BasedSharingAmount");
            var sharingAmount = metaModel.IsConvertAllToBaseCurrency ? basedSharingAmount : reader.GetVal<decimal>("SharingAmount");

            data.BetCount = reader.GetVal<int>("betcount");
            data.ProductCode = productCode;
            data.ProductName = ViewResources.ResourceManager.GetString(productCode);
            data.BasedTurnover = reader.GetVal<decimal>("BasedTurnover");
            data.BasedAmount = reader.GetVal<decimal>("BasedAmount");
            data.BasedSharingAmount = productCode == Product.TIP ? 0 : basedSharingAmount;
            data.Turnover = metaModel.IsConvertAllToBaseCurrency ? data.BasedTurnover : reader.GetVal<decimal>("Turnover");
            data.Amount = metaModel.IsConvertAllToBaseCurrency ? data.BasedAmount : reader.GetVal<decimal>("Amount");
            data.SharingAmount = productCode == Product.TIP ? 0 : sharingAmount;
            data.Currency = currency;
            data.CurrencyName = currencyList.FirstOrDefault(x => x.ID == currency)?.Name;
        }

        private static IEnumerable<ExtraPayoutDTO> GetSumDataList(IEnumerable<ExtraPayoutDTO> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new List<ExtraPayoutDTO>();
            }

            return dataList.GroupBy(d => d.Currency).Select(g =>
            {
                var item = g.FirstOrDefault() ?? new ExtraPayoutDTO();
                return new ExtraPayoutDTO
                {
                    Currency = item.Currency,
                    CurrencyName = item.CurrencyName,
                    BetCount = g.Sum(_ => _.BetCount),
                    Turnover = g.Sum(_ => _.BasedTurnover),
                    Amount = g.Sum(_ => _.BasedAmount),
                    SharingAmount = g.Sum(_ => _.BasedSharingAmount)
                };
            }).ToList();
        }

        private static ExtraPayoutDTO GetSumTotalData(IEnumerable<ExtraPayoutDTO> dataList, string baseCurrency)
        {
            if (dataList == null || !dataList.Any())
            {
                return new ExtraPayoutDTO();
            }

            return new ExtraPayoutDTO
            {
                CurrencyName = baseCurrency,
                BetCount = dataList.Sum(_ => _.BetCount),
                Turnover = dataList.Sum(_ => _.BasedTurnover),
                Amount = dataList.Sum(_ => _.BasedAmount),
                SharingAmount = dataList.Sum(_ => _.BasedSharingAmount)
            };
        }

        private static void UpdateSpCommonParamsAsync(IObjectDb db, ExtraPayoutMetaModel metaModel, BalanceUp balanceUpInfo)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency.ToUpperInvariant());
            db.SetParameterValue("fromdate", metaModel.DrillDownLevel == (int)ReportLevel.Main ? metaModel.FromDate.Date : metaModel.DrillDownFromDate.Date);
            db.SetParameterValue("todate", metaModel.DrillDownLevel == (int)ReportLevel.Main ? metaModel.ToDate.Date : metaModel.DrillDownToDate.Date);
            db.SetParameterValue("productList", metaModel.ProductCodeList);
            db.SetParameterValue("PromotionTypeList", metaModel.PromotionTypeIdList);
            db.SetParameterValue("movebetsday", balanceUpInfo.MoveBetsDay);
            db.SetParameterValue("BalanceUpDay", balanceUpInfo.BalanceUpDay);
        }

        private async Task<SortableResultModel<ExtraPayoutDTO>> GetMainDataAsync(
            IDataReader reader,
            ExtraPayoutMetaModel metaModel)
        {
            var dataList = new SortableList<ExtraPayoutDTO>();
            var isMonthly = metaModel.ReportBy == ReportByType.Monthly;
            var currencyListTask = await _currencyService.GetCurrencyListAsync();

            while (reader.Read())
            {
                var hasErrCode = reader.HasColumn("ErrCode");
                if (hasErrCode)
                {
                    return new SortableResultModel<ExtraPayoutDTO>
                    {
                        DataList = new SortableList<ExtraPayoutDTO>()
                    };
                }

                var date = ConvertHelper.ToDateTime(reader.GetVal<string>("reporttype"), isMonthly);
                var dateString = isMonthly ? date.ToString("MM/yyyy") : date.ToString("MM/dd/yyyy");

                var item = new ExtraPayoutDTO
                {
                    Date = date,
                    DateString = dateString
                };
                ReadCommonData(reader, item, metaModel, currencyListTask);

                dataList.Add(item);
            }

            var groupData = DoRestructureData(dataList.GroupBy(i => new { i.Date }));

            return new SortableResultModel<ExtraPayoutDTO>
            {
                DataList = groupData,
                TotalDataList = GetSumDataList(groupData),
                TotalData = GetSumTotalData(groupData, metaModel.BaseCurrency.ToUpperInvariant()),
                TotalRows = groupData.Count()
            };
        }

        private static IList<ExtraPayoutDTO> DoRestructureData(dynamic groupData, int index = 0)
        {
            var newDataList = new List<ExtraPayoutDTO>();

            foreach (var item in groupData)
            {
                var rowSpanFlag = true;
                var temps = (IList<ExtraPayoutDTO>)item;
                var numOfItem = temps.Count();

                var curencyRowSpanFlag = true;
                var currency = temps[0].Currency;

                var productRowSpanFlag = true;
                var product = temps[0].ProductCode;

                foreach (var col in temps)
                {
                    CalculateIndexRowSpan(col, numOfItem, ref rowSpanFlag, ref index);

                    CalculateProductRowSpan(temps, col, ref productRowSpanFlag, currency, ref product);

                    CalculateCurrencyRowSpan(temps, col, ref curencyRowSpanFlag, ref currency);

                    col.Index = index;
                    newDataList.Add(col);
                }
            }

            return newDataList;
        }

        private static void CalculateIndexRowSpan(ExtraPayoutDTO col, int numOfItem, ref bool rowSpanFlag, ref int index)
        {
            if (rowSpanFlag && numOfItem >= 1)
            {
                index++;
                col.IndexRowSpan = numOfItem;
                rowSpanFlag = false;
            }
            else
            {
                col.IndexRowSpan = -1;
            }
        }

        private static void CalculateProductRowSpan(IList<ExtraPayoutDTO> temps, ExtraPayoutDTO col, ref bool productRowSpanFlag, int currency, ref string product)
        {
            if (productRowSpanFlag || col.Currency != currency || col.ProductCode != product)
            {
                col.ProductRowSpan = temps.Count(_ => _.Currency == col.Currency && _.ProductCode == col.ProductCode);
                product = col.ProductCode;
                productRowSpanFlag = false;
            }
            else
            {
                col.ProductRowSpan = -1;
            }
        }

        private static void CalculateCurrencyRowSpan(IList<ExtraPayoutDTO> temps, ExtraPayoutDTO col, ref bool curencyRowSpanFlag, ref int currency)
        {
            if (curencyRowSpanFlag || col.Currency != currency)
            {
                col.CurencyRowSpan = temps.Count(_ => _.Currency == col.Currency);
                currency = col.Currency;
                curencyRowSpanFlag = false;
            }
            else
            {
                col.CurencyRowSpan = -1;
            }
        }

        public async Task<SortableResultModel<ExtraPayoutDTO>> GetExtraPayoutMemberData(
            ExtraPayoutMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_ExtraPayout_Mem";
            var balanceUpInfo = await _balanceUpService.GetBalanceUpAsync().ConfigureAwait(false);

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue("Username", metaModel.Username);
                UpdateSpCommonParamsAsync(db, metaModel, balanceUpInfo);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return await GetMemberDataAsync(reader, metaModel);
                }
            }
        }

        private async Task<SortableResultModel<ExtraPayoutDTO>> GetMemberDataAsync(
            IDataReader reader,
            ExtraPayoutMetaModel metaModel)
        {
            var dataList = new SortableList<ExtraPayoutDTO>();
            var isMonthly = metaModel.ReportBy == ReportByType.Monthly;
            var dateString = isMonthly ? metaModel.FromDate.ToString("MM/yyyy") : metaModel.FromDate.ToString("MM/dd/yyyy");
            var currencyListTask = await _currencyService.GetCurrencyListAsync();

            while (reader.Read())
            {
                var hasErrCode = reader.HasColumn("ErrCode");
                if (hasErrCode)
                {
                    return new SortableResultModel<ExtraPayoutDTO>
                    {
                        DataList = new SortableList<ExtraPayoutDTO>()
                    };
                }

                var item = new ExtraPayoutDTO
                {
                    Date = metaModel.FromDate.Date,
                    DateString = dateString
                };
                ReadCommonData(reader, item, metaModel, currencyListTask);
                ReadMemberData(reader, item);

                dataList.Add(item);
            }

            if (metaModel.IsExportExcel)
            {
                return new SortableResultModel<ExtraPayoutDTO>
                {
                    DataList = dataList,
                    TotalDataList = GetSumDataList(dataList),
                    TotalData = GetSumTotalData(dataList, metaModel.BaseCurrency.ToUpperInvariant()),
                    TotalRows = dataList.Count()
                };
            }

            var beginIndex = (metaModel.PageIndex - 1) * metaModel.PageSize;
            var rawGroupPagingData = dataList.GroupBy(i => new { i.CustId });

            var groupData = DoRestructureData(rawGroupPagingData.Skip(beginIndex).Take(metaModel.PageSize), beginIndex);

            return new SortableResultModel<ExtraPayoutDTO>
            {
                DataList = groupData,
                TotalDataList = GetSumDataList(groupData),
                TotalData = GetSumTotalData(groupData, metaModel.BaseCurrency.ToUpperInvariant()),
                TotalRows = rawGroupPagingData.Count()
            };
        }

        private void ReadMemberData(IDataReader reader, ExtraPayoutDTO data)
        {
            var promotionType = reader.GetVal<int>("PromotionType");

            data.CustId = reader.GetVal<int>("CustId");
            data.Username = GetUserNameValue(reader, promotionType);
            data.PromotionType = promotionType;
            data.PromotionName = GetPromotionName(promotionType);
            data.Nickname = GetNicknameValue(reader, promotionType);
        }

        private static string GetUserNameValue(IDataReader reader, int promotionType)
        {
            return promotionType == ExtraPayoutMapType.PayoutOddsBoostAdjustment ?
                ViewResources.OddsBoostBonus :
                reader.GetVal<string>("username");
        }

        private static string GetNicknameValue(IDataReader reader, int promotionType)
        {
            return promotionType == ExtraPayoutMapType.PayoutOddsBoostAdjustment ?
                string.Empty :
                reader.GetVal<string>("Nickname");
        }

        public async Task<SortableResultModel<ExtraPayoutDetailDTO>> GetExtraPayoutDetailData(
            ExtraPayoutDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_ExtraPayout_Mem_Detail";
            var balanceUpInfo = await _balanceUpService.GetBalanceUpAsync().ConfigureAwait(false);

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("BalanceUpDay", balanceUpInfo.BalanceUpDay);
                UpdateSpCommonParamsAsync(db, metaModel, balanceUpInfo);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return GetDetailData(reader, metaModel);
                }
            }
        }

        private SortableResultModel<ExtraPayoutDetailDTO> GetDetailData(
            IDataReader reader, ExtraPayoutDetailMetaModel metaModel)
        {
            var dataList = new SortableList<ExtraPayoutDetailDTO>();

            while (reader.Read())
            {
                var productCode = reader.GetVal<string>("ProductCode") ?? string.Empty;
                var promotionType = reader.GetVal<int>("PromotionType");
                var date = reader.GetVal<DateTime>("WinLostDate");
                var dateString = date.ToString("MM/dd/yyyy");
                var basedSharingAmount = reader.GetVal<decimal>("BasedSharingAmount");
                var sharingAmount = metaModel.IsConvertAllToBaseCurrency ? basedSharingAmount : reader.GetVal<decimal>("SharingAmount");
                var betID = promotionType == ExtraPayoutMapType.PayoutSabaRewardRiskFreeRebate ? reader.GetVal<string>("BetID") : "-";

                var item = new ExtraPayoutDetailDTO
                {
                    Date = date,
                    DateString = dateString,
                    ProductCode = productCode,
                    ProductName = ViewResources.ResourceManager.GetString(productCode),
                    PromotionType = promotionType,
                    PromotionName = GetPromotionName(promotionType),
                    BasedTurnover = reader.GetVal<decimal>("BasedTurnover"),
                    BasedAmount = reader.GetVal<decimal>("BasedAmount"),
                    BasedSharingAmount = productCode == Product.TIP ? 0 : basedSharingAmount,
                    Refno = reader.GetVal<string>("RefNo"),
                    BetID = betID
                };

                item.Turnover = metaModel.IsConvertAllToBaseCurrency ? item.BasedTurnover : reader.GetVal<decimal>("Turnover");
                item.Amount = metaModel.IsConvertAllToBaseCurrency ? item.BasedAmount : reader.GetVal<decimal>("Amount");
                item.SharingAmount = productCode == Product.TIP ? 0 : sharingAmount;

                dataList.Add(item);
            }

            return new SortableResultModel<ExtraPayoutDetailDTO>
            {
                DataList = dataList,
                TotalData = GetSumDetailData(dataList),
                TotalRows = dataList.Count()
            };
        }

        private static ExtraPayoutDetailDTO GetSumDetailData(List<ExtraPayoutDetailDTO> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new ExtraPayoutDetailDTO();
            }

            return new ExtraPayoutDetailDTO
            {
                Turnover = dataList.Sum(_ => _.BasedTurnover),
                Amount = dataList.Sum(_ => _.BasedAmount),
                SharingAmount = dataList.Sum(_ => _.BasedSharingAmount)
            };
        }

        private static bool IsSportType(int betType)
        {
            return betType == BetType.VgamingFreeBets || betType == BetType.SCBFreeBets;
        }

        public string GetPromotionName(int promotionType)
        {
            promotionType = _extraPayoutMapType.TryGetValue(promotionType, out int mapExtraPayoutType) ? mapExtraPayoutType : promotionType;

            if (SBPromotionBettypes.Contains(promotionType))
            {
                return $"{ViewResources.SBPromotion} - {_cacheRefAdapter.GetBettype(promotionType)}";
            }

            if (CasinoPromotionBettypes.Contains(promotionType))
            {
                return $"{ViewResources.CasinoPromotion} - {_cacheRefAdapter.GetBettype(promotionType)}";
            }

            if (promotionType == BetType.OddsBoostAdjustment)
            {
                return $"{ViewResources.SB} - {ViewResources.OddsBoostBonus}";
            }

            if (NumberGamePromotionBettypes.Contains(promotionType))
            {
                return $"{ViewResources.NGPromotion} - {_cacheRefAdapter.GetBettype(promotionType)}";
            }

            return IsSportType(promotionType) ? _cacheRefAdapter.GetSportName(promotionType) : _cacheRefAdapter.GetBettype(promotionType);
        }

        public async Task<SortableReportResultModel<OddsBoostBonusDetail>> GetOddsBoostBonusDetailData(
            ExtraPayoutMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_ExtraPayout_OddsBoost_Detail";
            var balanceUpInfo = await _balanceUpService.GetBalanceUpAsync().ConfigureAwait(false);

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency.ToUpperInvariant());
                db.SetParameterValue("fromdate", metaModel.DrillDownFromDate.Date);
                db.SetParameterValue("todate", metaModel.DrillDownToDate.Date);
                db.SetParameterValue("movebetsday", balanceUpInfo.MoveBetsDay);
                db.SetParameterValue("BalanceUpDay", balanceUpInfo.BalanceUpDay);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return GetOddsBoostBonusDetailData(reader, metaModel);
                }
            }
        }

        private SortableReportResultModel<OddsBoostBonusDetail> GetOddsBoostBonusDetailData(IDataReader reader, ExtraPayoutMetaModel metaModel)
        {
            var dataList = new SortableList<OddsBoostBonusDetail>();

            while (reader.Read())
            {
                var date = reader.GetVal<DateTime>("WinLostDate");
                var dateString = date.ToString("MM/dd/yyyy");
                var productCode = reader.GetVal<string>("ProductCode");

                var item = new OddsBoostBonusDetail
                {
                    Date = date,
                    DateString = dateString,
                    ExternalId = reader.GetVal<string>("ExternalId"),
                    SystemId = reader.GetVal<string>("SystemId"),
                    RefNo = reader.GetVal<string>("RefNo"),
                    ProductCode = productCode,
                    ProductName = DataHelper.GetProductName(productCode),
                    PromotionType = ViewResources.OddsBoostBonus,
                    BasedCustomerAmount = reader.GetVal<decimal>("BasedCustomerAmount")
                };

                item.CustomerAmount = metaModel.IsConvertAllToBaseCurrency ? item.BasedCustomerAmount : reader.GetVal<decimal>("CustomerAmount");

                dataList.Add(item);
            }

            return new SortableReportResultModel<OddsBoostBonusDetail>
            {
                DataList = dataList,
                TotalData = GetSumOddsBoostBonusDetailData(dataList)
            };
        }

        private static OddsBoostBonusDetail GetSumOddsBoostBonusDetailData(List<OddsBoostBonusDetail> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new OddsBoostBonusDetail();
            }

            return new OddsBoostBonusDetail
            {
                CustomerAmount = dataList.Sum(_ => _.BasedCustomerAmount)
            };
        }
    }
}