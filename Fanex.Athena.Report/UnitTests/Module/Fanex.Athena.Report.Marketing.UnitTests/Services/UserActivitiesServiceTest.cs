using AutoFixture;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.Enums;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using Fanex.Athena.Report.Marketing.ViewModels;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Services
{
    public class UserActivitiesServiceTest
    {
        private readonly UserActivitiesService _targetService;
        private readonly Mock<IUserActivitiesRepository> _mockRepository;
        private readonly IFixture _fixture = new Fixture();

        public UserActivitiesServiceTest()
        {
            _mockRepository = new Mock<IUserActivitiesRepository>();
            _targetService = new UserActivitiesService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetData_DayOfWeekViewMode_DisplayTimeAsDayOfWeek()
        {
            //Arrange
            var metaModel = _fixture.Create<UserActivitiesMetaModel>();
            metaModel.ViewMode = (byte)ViewMode.Daily;
            metaModel.FromDate = new DateTime(2020, 8, 4);
            metaModel.ToDate = metaModel.FromDate.AddDays(-6);

            var (rawData, expectedResult) = FakeDataForDayOfWeekViewMode();

            _mockRepository.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult<IEnumerable<UserActivitiesDTO>>(rawData));

            //Act
            var actualResult = await _targetService.GetData(metaModel);

            //Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetData_NormalMode_DisplayTimeAsRawData(byte viewMode)
        {
            //Arrange
            var metaModel = _fixture.Create<UserActivitiesMetaModel>();
            metaModel.ViewMode = (byte)ViewMode.Daily;
            metaModel.FromDate = new DateTime(2020, 8, 4);
            metaModel.ToDate = metaModel.FromDate.AddDays(-29);
            metaModel.ViewMode = viewMode;

            var dto = new UserActivitiesDTO
            {
                ViewBy = _fixture.Create<string>(),
                ActiveUsers = _fixture.Create<int>(),
                NewlyRegisterUsers = _fixture.Create<int>(),
            };

            var viewModel = new UserActivitiesViewModel
            {
                Time = dto.ViewBy,
                ActiveUsers = dto.ActiveUsers,
                NewlyRegisterUsers = dto.NewlyRegisterUsers
            };

            var rawData = new List<UserActivitiesDTO>() { dto };
            var expectedResult = new List<UserActivitiesViewModel> { viewModel };

            rawData.Add(dto);
            expectedResult.Add(viewModel);

            _mockRepository.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult<IEnumerable<UserActivitiesDTO>>(rawData));

            //Act
            var actualResult = await _targetService.GetData(metaModel);

            //Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private static (IEnumerable<UserActivitiesDTO> rawData, IEnumerable<UserActivitiesViewModel> viewModel) FakeDataForDayOfWeekViewMode()
        {
            var rawData = new List<UserActivitiesDTO>
            {
                new UserActivitiesDTO
                {
                    ViewBy = "8/4/2020",
                    ActiveUsers = 100,
                    NewlyRegisterUsers = 123
                },
                new UserActivitiesDTO
                {
                    ViewBy = "8/3/2020",
                    ActiveUsers = 5512,
                    NewlyRegisterUsers = 1235
                },
                new UserActivitiesDTO
                {
                    ViewBy = "8/2/2020",
                    ActiveUsers = 0,
                    NewlyRegisterUsers = 1
                },
                new UserActivitiesDTO
                {
                    ViewBy = "8/1/2020",
                    ActiveUsers = 23,
                    NewlyRegisterUsers = 54
                },
                new UserActivitiesDTO
                {
                    ViewBy = "7/31/2020",
                    ActiveUsers = 123,
                    NewlyRegisterUsers = 90
                },
                new UserActivitiesDTO
                {
                    ViewBy = "7/30/2020",
                    ActiveUsers = 66,
                    NewlyRegisterUsers = 34
                },
                new UserActivitiesDTO
                {
                    ViewBy = "7/29/2020",
                    ActiveUsers = 1266,
                    NewlyRegisterUsers = 16550
                }
            };

            var viewModel = new List<UserActivitiesViewModel>
            {
                new UserActivitiesViewModel
                {
                    Time = "Tuesday",
                    ActiveUsers = 100,
                    NewlyRegisterUsers = 123
                },
                new UserActivitiesViewModel
                {
                    Time = "Monday",
                    ActiveUsers = 5512,
                    NewlyRegisterUsers = 1235
                },
                new UserActivitiesViewModel
                {
                    Time = "Sunday",
                    ActiveUsers = 0,
                    NewlyRegisterUsers = 1
                },
                new UserActivitiesViewModel
                {
                    Time = "Saturday",
                    ActiveUsers = 23,
                    NewlyRegisterUsers = 54
                },
                new UserActivitiesViewModel
                {
                    Time = "Friday",
                    ActiveUsers = 123,
                    NewlyRegisterUsers = 90
                },
                new UserActivitiesViewModel
                {
                    Time = "Thursday",
                    ActiveUsers = 66,
                    NewlyRegisterUsers = 34
                },
                new UserActivitiesViewModel
                {
                    Time = "Wednesday",
                    ActiveUsers = 1266,
                    NewlyRegisterUsers = 16550
                }
            };

            return (rawData, viewModel);
        }
    }
}