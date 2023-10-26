using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.BonusInfo.Repositories;
using Fanex.Athena.Report.Common.Consts;
using Fanex.Athena.Report.Common.Extensions;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.NVC.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.BonusInfo.Services
{
    public class BonusInfoService : IBonusInfoService
    {
        private readonly IBonusInfoRepository _repository;
        private readonly IDataPermissionService _dataPermissionService;
        private readonly INVCPermissionService _permissionService;
        private readonly INVCAuthInfoService _authInfoService;

        public BonusInfoService(
            IBonusInfoRepository repository,
            IDataPermissionService dataPermissionService,
            INVCPermissionService permissionService,
            INVCAuthInfoService authInfoService)
        {
            _repository = repository;
            _dataPermissionService = dataPermissionService;
            _permissionService = permissionService;
            _authInfoService = authInfoService;
        }

        public async Task<BonusInfoSummaryDTO> GetSummaryData(BonusInfoSummaryMetaModel metaModel)
        {
            var data = await _repository.GetSummaryData(metaModel);

            var sortAbleList = data.SortedData.DataList as ISortableList<BonusInfoSummaryItemDTO>;
            sortAbleList?.Sort(metaModel.SortBy, metaModel.SortType);

            data.SortedData.DataList = sortAbleList;

            return ClearSummaryData(data);
        }

        private BonusInfoSummaryDTO ClearSummaryData(BonusInfoSummaryDTO data)
        {
            data.SortedData.DataList.ToList().ForEach(_ => _dataPermissionService.ClearDataByPermission(_));
            _dataPermissionService.ClearDataByPermission(data.SortedData.TotalData);

            return data;
        }

        public async Task<BonusInfoDetailDTO> GetDetailsData(BonusInfoDetailMetaModel metaModel)
        {
            var data = await _repository.GetDetailsData(metaModel);
            return ClearDetailsData(data);
        }

        private BonusInfoDetailDTO ClearDetailsData(BonusInfoDetailDTO data)
        {
            data.DataList.ToList().ForEach(_ => _dataPermissionService.ClearDataByPermission(_));

            return data;
        }

        public async Task<IEnumerable<MultipleSelectOption>> GetBonusTypes()
        {
            var bonusTypes = await _repository.GetBonusTypes();
            bonusTypes = _authInfoService.Level != (int)Constant.Level.User ? bonusTypes
                : bonusTypes.Where(_ => _permissionService.HasPermission(PermissionKeys.BonusTypePrefix + _.BonusTypeId.ToString()));

            return bonusTypes.Select(_ => new MultipleSelectOption { Key = _.BonusTypeId, Value = _.BonusTypeName, Status = true });
        }
    }
}