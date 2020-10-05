using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using System.Threading.Tasks;
using CommonExtensions = Fanex.Athena.Report.Common.Extensions;

namespace Fanex.Athena.Report.Marketing.Services
{
    public class WithdrawalService : IWithdrawalService
    {
        private readonly IWithdrawalRepository _repository;
        private readonly ICalculationService _calculationService;

        public WithdrawalService(IWithdrawalRepository repository, ICalculationService calculationService)
        {
            _repository = repository;
            _calculationService = calculationService;
        }

        public async Task<WithdrawalSummaryDTO> GetSummaryData(WithdrawalSummaryMetaModel metaModel)
        {
            var rawData = await _repository.GetSummaryData(metaModel);
            HandleSummaryRawData(rawData);
            return rawData;
        }

        public async Task<ResultModel<WithdrawalDetailDTO>> GetDetailsData(WithdrawalDetailMetaModel metaModel)
        {
            var (rows, totalRows) = await _repository.GetDetailsData(metaModel);
            return new ResultModel<WithdrawalDetailDTO>
            {
                DataList = rows,
                TotalRows = totalRows
            };
        }

        public async Task<(WithdrawalSummaryDTO, ResultModel<WithdrawalDetailDTO>)> GetExportData(WithdrawalExportMetaModel metaModel)
        {
            var summaryMetaModel = new WithdrawalSummaryMetaModel
            {
                BrandId = metaModel.BrandId,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                CompareFromDate = metaModel.CompareFromDate,
                CompareToDate = metaModel.CompareToDate,
                BaseCurrency = metaModel.BaseCurrency
            };

            var detailMetaModel = new WithdrawalDetailMetaModel
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

        private void HandleSummaryRawData(WithdrawalSummaryDTO rawData)
        {
            if (rawData != null)
            {
                rawData.WithdrawalAmountChangeRate = _calculationService.CalculateGrowth((double)rawData.WithdrawalAmount, (double)rawData.CompareWithdrawalAmount);
                rawData.WithdrawalUsersChangeRate = _calculationService.CalculateGrowth(rawData.WithdrawalUsers, rawData.CompareWithdrawalUsers);
            }
        }
    }
}