using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.ViewModels;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public class PerformanceHighlightService : IPerformanceHighlightService
    {
        private readonly IPerformanceHighlightRepository _repository;
        private readonly ICalculationService _calculationService;

        public PerformanceHighlightService(
            IPerformanceHighlightRepository repository,
            ICalculationService calculationService)
        {
            _repository = repository;
            _calculationService = calculationService;
        }

        public async Task<PerformanceHighlightViewModel> GetData(PerformanceHighlightMetaModel metaModel)
        {
            var rawData = await _repository.GetData(metaModel);
            var result = HandleData(rawData);
            return result;
        }

        public async Task<PerformanceHighlightExportDTO> ExportData(PerformanceHighlightExportAllMetaModel metaModel)
        {
            var data = await _repository.ExportData(metaModel);
            return data;
        }

        private PerformanceHighlightViewModel HandleData(PerformanceHighlightDTO rawData)
        {
            var data = new PerformanceHighlightViewModel();

            data.TotalUsers = rawData.TotalUsers;

            data.TotalDepositUsers = rawData.TotalDepositUsers;

            data.TotalActiveUsers = rawData.TotalActiveUsers;

            data.DepositUsers = rawData.DepositUsers;
            data.GrowthRateDepositUsers = _calculationService.CalculateGrowth(rawData.DepositUsers, rawData.CompareDepositUsers);

            data.ActiveUsers = rawData.ActiveUsers;
            data.GrowthRateActiveUsers = _calculationService.CalculateGrowth(rawData.ActiveUsers, rawData.CompareActiveUsers);

            data.NewlyRegisterUsers = rawData.NewlyRegisterUsers;
            data.GrowthRateNewlyRegisterUsers = _calculationService.CalculateGrowth(rawData.NewlyRegisterUsers, rawData.CompareNewlyRegisterUsers);

            data.NewlyActiveRegisterUsers = rawData.NewlyActiveRegisterUsers;
            data.GrowthRateNewlyActiveRegisterUsers = _calculationService.CalculateGrowth(rawData.NewlyActiveRegisterUsers, rawData.CompareNewlyActiveRegisterUsers);

            data.DepositAmount = rawData.DepositAmount;
            data.GrowthRateDepositAmount = _calculationService.CalculateGrowth((double)rawData.DepositAmount, (double)rawData.CompareDepositAmount);

            data.WithdrawalAmount = rawData.WithdrawalAmount;
            data.GrowthRateWithdrawalAmount = _calculationService.CalculateGrowth((double)rawData.WithdrawalAmount, (double)rawData.CompareWithdrawalAmount);

            data.BonusAmountClaimeds = rawData.BonusAmountClaimeds;
            data.GrowthRateBonusAmountClaimeds = _calculationService.CalculateGrowth((double)rawData.BonusAmountClaimeds, (double)rawData.CompareBonusAmountClaimeds);

            data.ChurnRate = _calculationService.CalculateDistribution(rawData.UptoTotalUsers - rawData.ActiveUsers, rawData.UptoTotalUsers);
            var compareChurnRate = _calculationService.CalculateDistribution(rawData.CompareUptoTotalUsers - rawData.CompareActiveUsers, rawData.CompareUptoTotalUsers);
            data.DifferentChurnRate = data.ChurnRate - compareChurnRate;

            const int fullPercent = 100;
            data.RetentionRate = fullPercent - data.ChurnRate;
            data.DifferentRetentionRate = -data.DifferentChurnRate;

            return data;
        }
    }
}