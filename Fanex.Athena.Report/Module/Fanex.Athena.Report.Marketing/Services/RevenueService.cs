using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public class RevenueService : IRevenueService
    {
        private readonly IRevenueRepository _repository;
        private readonly IProductService _productService;
        private readonly IDataMapper _dataMapper;

        public RevenueService(IRevenueRepository repository,
            IProductService productService,
            IDataMapper dataMapper)
        {
            _repository = repository;
            _productService = productService;
            _dataMapper = dataMapper;
        }

        public async Task<IEnumerable<AvgRevenueViewModel>> GetAvgData(AvgRevenueMetaModel metaModel)
        {
            var rawData = await _repository.GetAvgData(metaModel);
            var data = await HandleRawData(rawData);
            return data;
        }

        private async Task<IEnumerable<AvgRevenueViewModel>> HandleRawData(IEnumerable<AvgRevenueDTO> rawData)
        {
            var tpProducts = await _productService.GetDictionaryTpProductAsync();
            return rawData.Select(rawModel =>
            {
                var model = new AvgRevenueViewModel
                {
                    ProductId = rawModel.ProductId,
                    AllUsersRevenue = rawModel.Turnover,
                    PaidUsersRevenue = rawModel.PaidUserTurnover
                };

                BindProductName(model, rawModel, tpProducts);

                return model;
            });
        }

        private void BindProductName(AvgRevenueViewModel model, AvgRevenueDTO rawModel, IDictionary<int, TpProduct> tpProducts)
        {
            model.ProductName = _dataMapper.ToTpProductName(tpProducts, rawModel.ProductId);
        }
    }
}