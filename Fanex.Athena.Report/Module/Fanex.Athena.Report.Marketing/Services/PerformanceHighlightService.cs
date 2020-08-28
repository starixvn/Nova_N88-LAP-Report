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

        private PerformanceHighlightViewModel HandleData(PerformanceHighlightDTO rawData)
        {
            var data = new PerformanceHighlightViewModel();

            data.TotalUsers = rawData.TotalUsers;

            data.PaidUsers = rawData.PaidUsers;

            data.ActiveUsers = rawData.ActiveUsers;
            data.GrowthRateActiveUsers = _calculationService.CalculateGrowth(rawData.ActiveUsers, rawData.CompareActiveUsers);

            data.NewlyRegisterUsers = rawData.NewlyRegisterUsers;
            data.GrowthRateNewlyRegisterUsers = _calculationService.CalculateGrowth(rawData.NewlyRegisterUsers, rawData.CompareNewlyRegisterUsers);

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