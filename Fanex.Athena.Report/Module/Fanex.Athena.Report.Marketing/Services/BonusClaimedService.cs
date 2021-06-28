using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonExtensions = Fanex.Athena.Report.Common.Extensions;

namespace Fanex.Athena.Report.Marketing.Services
{
    public class BonusClaimedService : IBonusClaimedService
    {
        private readonly IBonusClaimedRepository _repository;
        private readonly ICalculationService _calculationService;
        private readonly IDataMapper _dataMapper;
        private readonly ICurrencyService _currencyService;

        public BonusClaimedService(
            IBonusClaimedRepository repository,
            ICalculationService calculationService,
            ICurrencyService currencyService,
            IDataMapper dataMapper)
        {
            _repository = repository;
            _calculationService = calculationService;
            _currencyService = currencyService;
            _dataMapper = dataMapper;
        }

        public async Task<BonusClaimedSummaryDTO> GetSummaryData(BonusClaimedSummaryMetaModel metaModel)
        {
            var rawData = await _repository.GetSummaryData(metaModel);
            await HandleSummaryRawData(rawData);
            return rawData;
        }

        public async Task<ResultModel<BonusClaimedDetailDTO>> GetDetailsData(BonusClaimedDetailMetaModel metaModel)
        {
            var (rows, totalRows) = await _repository.GetDetailsData(metaModel);
            return new ResultModel<BonusClaimedDetailDTO>
            {
                DataList = rows,
                TotalRows = totalRows
            };
        }

        public async Task<(BonusClaimedSummaryDTO, ResultModel<BonusClaimedDetailDTO>)> GetExportData(BonusClaimedExportMetaModel metaModel)
        {
            var summaryMetaModel = new BonusClaimedSummaryMetaModel
            {
                BrandId = metaModel.BrandId,
                CurrencyIdList = metaModel.CurrencyIdList,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                CompareFromDate = metaModel.CompareFromDate,
                CompareToDate = metaModel.CompareToDate,
                BaseCurrency = metaModel.BaseCurrency
            };

            var detailMetaModel = new BonusClaimedDetailMetaModel
            {
                BrandId = metaModel.BrandId,
                CurrencyIdList = metaModel.CurrencyIdList,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                BaseCurrency = metaModel.BaseCurrency,
                SortbyId = metaModel.SortbyId,
                IsSortAsc = metaModel.IsSortAsc,
                PageIndex = 1,
                PageSize = int.MaxValue - 100
            };
            var (summaryData, detailData) = await CommonExtensions.TaskExtensions.WhenAll(GetSummaryData(summaryMetaModel), GetDetailsData(detailMetaModel));

            return (summaryData, detailData);
        }

        private async Task HandleSummaryRawData(BonusClaimedSummaryDTO rawData)
        {
            var currencies = await _currencyService.GetCurrencyDictionaryAsync();
            HandleSummaryTotalRawData(rawData, currencies);
            HandleSummaryDetailRawData(rawData, currencies);
        }

        private void HandleSummaryTotalRawData(BonusClaimedSummaryDTO rawData, IDictionary<int, Currency> currencies)
        {
            var totalBonusAmountClaimeds = 0.0m;
            var totalCompareBonusAmountClaimeds = 0.0m;

            foreach (var item in rawData.TotalData)
            {
                item.ChangeRate = _calculationService.CalculateGrowth((double)item.BonusAmountClaimeds, (double)item.CompareBonusAmountClaimeds);
                item.CurrencyCode = _dataMapper.ToCurrencyCode(currencies, item.CurrencyId);
                totalBonusAmountClaimeds += item.BasedBonusAmountClaimeds;
                totalCompareBonusAmountClaimeds += item.CompareBasedBonusAmountClaimeds;
            }

            rawData.TotalTotalData.BonusAmountClaimeds = totalBonusAmountClaimeds;
            rawData.TotalTotalData.CompareBonusAmountClaimeds = totalCompareBonusAmountClaimeds;
            rawData.TotalTotalData.ChangeRate = _calculationService.CalculateGrowth((double)totalBonusAmountClaimeds, (double)totalCompareBonusAmountClaimeds);
        }

        private void HandleSummaryDetailRawData(BonusClaimedSummaryDTO rawData, IDictionary<int, Currency> currencies)
        {
            rawData.TotalDetailData.TotalBonusClaimeds = rawData.DetailData.Sum(_ => _.NumberOfBonusClaimed);
            rawData.TotalDetailData.TotalBonusAmount = rawData.DetailData.Sum(_ => _.BasedBonusAmount);
            rawData.DetailData = rawData.DetailData.GroupBy(_ => _.BonusCode,
                _ =>
                {

                    _.CurrencyCode = _dataMapper.ToCurrencyCode(currencies, _.CurrencyId);
                    return _;
                },
                (bonusCode, childs) =>
                {
                    return new DetailBonusClaimedDTO
                    {
                        BonusCode = bonusCode,
                        BonusName = childs.First().BonusName,
                        NumberOfBonusClaimed = childs.Sum(_ => _.NumberOfBonusClaimed),
                        BonusAmount = childs.Sum(_ => _.BasedBonusAmount),
                        Childs = childs
                    };
                });
        }
    }
}