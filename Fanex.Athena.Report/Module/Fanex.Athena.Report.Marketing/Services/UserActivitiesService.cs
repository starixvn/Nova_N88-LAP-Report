using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.ViewModels;
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
            var data = HandleRawData(rawData);
            return data;
        }

        private IEnumerable<UserActivitiesViewModel> HandleRawData(IEnumerable<UserActivitiesDTO> rawData)
        {
            return rawData.Select(rawModel =>
            {
                var viewModel = new UserActivitiesViewModel
                {
                    ActiveUsers = rawModel.ActiveUsers,
                    NewlyRegisterUsers = rawModel.NewlyRegisterUsers,
                    Time = rawModel.ViewBy
                };

                return viewModel;
            });
        }
    }
}