using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Helpers;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonExtensions = Fanex.Athena.Report.Common.Extensions;

namespace Fanex.Athena.Report.Marketing.Services
{
    public class NewlyRegisterUsersService : INewlyRegisterUsersService
    {
        private readonly INewlyRegisterUsersRepository _repository;
        private readonly ICalculationService _calculationService;

        public NewlyRegisterUsersService(INewlyRegisterUsersRepository repository, ICalculationService calculationService)
        {
            _repository = repository;
            _calculationService = calculationService;
        }

        public async Task<NewlyRegisterUsersSummaryDTO> GetSummaryData(NewlyRegisterUsersSummaryMetaModel metaModel)
        {
            var rawData = await _repository.GetSummaryData(metaModel);
            HandleSummaryRawData(rawData);
            return rawData;
        }

        public async Task<ResultModel<NewlyRegisterUsersDetailDTO>> GetDetailsData(NewlyRegisterUsersDetailMetaModel metaModel)
        {
            var (rows, totalRows) = await _repository.GetDetailsData(metaModel);
            return new ResultModel<NewlyRegisterUsersDetailDTO>
            {
                DataList = rows,
                TotalRows = totalRows
            };
        }

        public async Task<(NewlyRegisterUsersSummaryDTO, ResultModel<NewlyRegisterUsersDetailDTO>)> GetExportData(NewlyRegisterUsersExportMetaModel metaModel)
        {
            var summaryMetaModel = new NewlyRegisterUsersSummaryMetaModel
            {
                BrandId = metaModel.BrandId,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                CompareFromDate = metaModel.CompareFromDate,
                CompareToDate = metaModel.CompareToDate,
                BaseCurrency = metaModel.BaseCurrency
            };

            var detailMetaModel = new NewlyRegisterUsersDetailMetaModel
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

        private void HandleSummaryRawData(NewlyRegisterUsersSummaryDTO rawData)
        {
            var totalData = rawData.TotalData.FirstOrDefault();
            if (totalData != null)
            {
                totalData.ChangeRate = _calculationService.CalculateGrowth(totalData.NewlyRegisterUsers, totalData.CompareNewlyRegisterUsers);
            }


            rawData.TotalSignupData.TotalNewlyRegisterUsers = rawData.SignupData.Sum(_ => _.NewlyRegisterUsers);
            rawData.TotalSignupData.TotalDepositUsers = rawData.SignupData.Sum(_ => _.DepositUsers);
            rawData.TotalSignupData.TotalFirstDepositAmount = rawData.SignupData.Sum(_ => _.FirstDepositAmount);

            rawData.SignupData = GroupingHelper.GroupingSignupData(rawData.SignupData, (item, childs) =>
            {
                item.NewlyRegisterUsers = childs.Sum(_ => _.NewlyRegisterUsers);
                item.DepositUsers = childs.Sum(_ => _.DepositUsers);
                item.FirstDepositAmount = childs.Sum(_ => _.FirstDepositAmount);
            });
        }
    }
}