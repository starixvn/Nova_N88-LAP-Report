using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Enums;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.ViewModels;
using Fanex.Athena.Report.Resources.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Marketing.Services
{
    public class UserActivitiesService : IUserActivitiesService
    {
        private readonly IUserActivitiesRepository _repository;

        public UserActivitiesService(IUserActivitiesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserActivitiesViewModel>> GetData(UserActivitiesMetaModel metaModel)
        {
            var rawData = await _repository.GetData(metaModel);
            var data = HandleRawData(rawData, metaModel);
            return data;
        }

        private IEnumerable<UserActivitiesViewModel> HandleRawData(IEnumerable<UserActivitiesDTO> rawData, UserActivitiesMetaModel metaModel)
        {
            var isDayOfWeekViewMode = IsDayOfWeekViewMode(metaModel);
            return rawData.Select(rawModel =>
            {
                var viewModel = new UserActivitiesViewModel
                {
                    ActiveUsers = rawModel.ActiveUsers,
                    NewlyRegisterUsers = rawModel.NewlyRegisterUsers,
                };

                BindTimeDisplay(viewModel, rawModel, isDayOfWeekViewMode);

                return viewModel;
            });
        }

        private static void BindTimeDisplay(UserActivitiesViewModel model, UserActivitiesDTO rawModel, bool isDayOfWeekViewMode) =>
            model.Time = isDayOfWeekViewMode && DateTime.TryParse(rawModel.ViewBy, out DateTime date)
                ? DataHelper.GetValue($"{nameof(date.DayOfWeek)}_{(byte)date.DayOfWeek}")
                : rawModel.ViewBy;

        private static bool IsDayOfWeekViewMode(UserActivitiesMetaModel metaModel)
        {
            const int MaxDiffDaysToViewDayOfWeek = 7;
            var diffDays = metaModel.ToDate.Subtract(metaModel.FromDate).Days;
            return metaModel.ViewMode == (byte)ViewMode.Daily && diffDays < MaxDiffDaysToViewDayOfWeek;
        }
    }
}