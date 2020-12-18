using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Helpers;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using System.Linq;
using System.Threading.Tasks;
using CommonExtensions = Fanex.Athena.Report.Common.Extensions;

namespace Fanex.Athena.Report.Marketing.Services
{
    public class DepositAmountService : IDepositAmountService
    {
        private readonly IDepositAmountRepository _repository;
        private readonly ICalculationService _calculationService;

        public DepositAmountService(IDepositAmountRepository repository, ICalculationService calculationService)
        {
            _repository = repository;
            _calculationService = calculationService;
        }

        public async Task<DepositAmountSummaryDTO> GetSummaryData(DepositAmountSummaryMetaModel metaModel)
        {
            var rawData = await _repository.GetSummaryData(metaModel);
            HandleSummaryRawData(rawData);
            return rawData;
        }

        public async Task<ResultModel<DepositAmountDetailDTO>> GetDetailsData(DepositAmountDetailMetaModel metaModel)
        {
            var (rows, totalRows) = await _repository.GetDetailsData(metaModel);
            return new ResultModel<DepositAmountDetailDTO>
            {
                DataList = rows,
                TotalRows = totalRows
            };
        }

        public async Task<(DepositAmountSummaryDTO, ResultModel<DepositAmountDetailDTO>)> GetExportData(DepositAmountExportMetaModel metaModel)
        {
            var summaryMetaModel = new DepositAmountSummaryMetaModel
            {
                BrandId = metaModel.BrandId,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                CompareFromDate = metaModel.CompareFromDate,
                CompareToDate = metaModel.CompareToDate,
                BaseCurrency = metaModel.BaseCurrency
            };

            var detailMetaModel = new DepositAmountDetailMetaModel
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

        private void HandleSummaryRawData(DepositAmountSummaryDTO rawData)

        {
            var totalData = rawData.TotalData.FirstOrDefault();
            if (totalData != null)
            {
                totalData.ChangeRate = _calculationService.CalculateGrowth((double)totalData.DepositAmount, (double)totalData.CompareDepositAmount);
            }

            rawData.TotalSignupData.TotalFirstDepositAmount = rawData.SignupData.Sum(_ => _.FirstDepositAmount);
            rawData.TotalSignupData.TotalDepositAmount = rawData.SignupData.Sum(_ => _.DepositAmount);

            rawData.SignupData = GroupingHelper.GroupingSignupData(rawData.SignupData, (item, childs) =>
            {
                item.FirstDepositAmount = childs.Sum(_ => _.FirstDepositAmount);
                item.DepositAmount = childs.Sum(_ => _.DepositAmount);
            });
        }
    }
}