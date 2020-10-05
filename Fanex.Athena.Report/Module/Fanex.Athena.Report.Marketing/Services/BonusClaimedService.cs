using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using System.Linq;
using System.Threading.Tasks;
using CommonExtensions = Fanex.Athena.Report.Common.Extensions;

namespace Fanex.Athena.Report.Marketing.Services
{
    public class BonusClaimedService : IBonusClaimedService
    {
        private readonly IBonusClaimedRepository _repository;
        private readonly ICalculationService _calculationService;

        public BonusClaimedService(IBonusClaimedRepository repository, ICalculationService calculationService)
        {
            _repository = repository;
            _calculationService = calculationService;
        }

        public async Task<BonusClaimedSummaryDTO> GetSummaryData(BonusClaimedSummaryMetaModel metaModel)
        {
            var rawData = await _repository.GetSummaryData(metaModel);
            HandleSummaryRawData(rawData);
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
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                CompareFromDate = metaModel.CompareFromDate,
                CompareToDate = metaModel.CompareToDate,
                BaseCurrency = metaModel.BaseCurrency
            };

            var detailMetaModel = new BonusClaimedDetailMetaModel
            {
                BrandId = metaModel.BrandId,
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

        private void HandleSummaryRawData(BonusClaimedSummaryDTO rawData)
        {
            var totalData = rawData.TotalData.FirstOrDefault();
            if (totalData != null)
            {
                totalData.ChangeRate = _calculationService.CalculateGrowth(totalData.BonusClaimeds, totalData.CompareBonusClaimeds);
            }

            rawData.TotalDetailData.TotalBonusClaimeds = rawData.DetailData.Sum(_ => _.NumberOfBonusClaimed);
            rawData.TotalDetailData.TotalBonusAmount = rawData.DetailData.Sum(_ => _.BonusAmount);
        }
    }
}