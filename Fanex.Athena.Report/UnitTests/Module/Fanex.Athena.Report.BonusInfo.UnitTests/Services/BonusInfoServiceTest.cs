using AutoFixture;
using Fanex.Athena.Report.BonusInfo.DTO;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.BonusInfo.Repositories;
using Fanex.Athena.Report.BonusInfo.Services;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.NVC.Authentication;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.BonusInfo.UnitTests.Services
{
    public class BonusInfoServiceTest
    {
        private readonly BonusInfoService _targetService;
        private readonly Mock<IBonusInfoRepository> _mockRepository;
        private readonly Mock<IDataPermissionService> _dataService;
        private readonly Mock<INVCPermissionService> _permissionService;
        private readonly Mock<INVCAuthInfoService> _authInfoService;
        private readonly IFixture _fixture = new Fixture();

        public BonusInfoServiceTest()
        {
            _mockRepository = new Mock<IBonusInfoRepository>();
            _dataService = new Mock<IDataPermissionService>();
            _permissionService = new Mock<INVCPermissionService>();
            _authInfoService = new Mock<INVCAuthInfoService>();
            _targetService = new BonusInfoService(_mockRepository.Object, _dataService.Object, _permissionService.Object, _authInfoService.Object);
        }

        [Fact]
        public async Task GetSummaryData_WhenCall_ReturnDataCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<BonusInfoSummaryMetaModel>();
            var expectedResult = _fixture.Create<BonusInfoSummaryDTO>();

            _mockRepository.Setup(_ => _.GetSummaryData(metaModel)).Returns(Task.FromResult(expectedResult));
            //Act
            var actualResult = await _targetService.GetSummaryData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetSummaryData_WhenCall_SortThenReturnDataCorrectly()
        {
            //Arrange
            var metaModel = new BonusInfoSummaryMetaModel
            {
                SortBy = "bonusCode",
                SortType = "asc"
            };

            var rawDataList = new List<BonusInfoSummaryItemDTO>
            {
                new BonusInfoSummaryItemDTO
                {
                    BonusCode = "gdc123",
                    BonusName = "gdc123"
                },
                new BonusInfoSummaryItemDTO
                {
                    BonusCode = "abc123",
                    BonusName = "abc123"
                },
                new BonusInfoSummaryItemDTO
                {
                    BonusCode = "zxy987",
                    BonusName = "zxy987"
                }
            };

            var rawData = new BonusInfoSummaryDTO
            {
                SortedData = new SortableResultModel<BonusInfoSummaryItemDTO>
                {
                    DataList = rawDataList,
                    TotalRows = rawDataList.Count
                },
            };

            var sortedDataList = new List<BonusInfoSummaryItemDTO>
            {
                new BonusInfoSummaryItemDTO
                {
                    BonusCode = "abc123",
                    BonusName = "abc123"
                },
                new BonusInfoSummaryItemDTO
                {
                    BonusCode = "gdc123",
                    BonusName = "gdc123"
                },
                new BonusInfoSummaryItemDTO
                {
                    BonusCode = "zxy987",
                    BonusName = "zxy987"
                }
            };
            var expectedResult = new BonusInfoSummaryDTO
            {
                SortedData = new SortableResultModel<BonusInfoSummaryItemDTO>
                {
                    DataList = sortedDataList,
                    TotalRows = sortedDataList.Count
                },
            };

            _mockRepository.Setup(_ => _.GetSummaryData(metaModel)).Returns(Task.FromResult(rawData));
            //Act
            var actualResult = await _targetService.GetSummaryData(metaModel);

            //Assert
            var actualResultJson = JsonConvert.SerializeObject(actualResult);
            var expectedResultJson = JsonConvert.SerializeObject(expectedResult);

            Assert.Equal(expectedResultJson, actualResultJson);
        }

        [Fact]
        public async Task GetDetailsData_WhenCall_ReturnDataCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<BonusInfoDetailMetaModel>();
            var expectedResult = _fixture.Create<BonusInfoDetailDTO>();

            _mockRepository.Setup(_ => _.GetDetailsData(metaModel)).Returns(Task.FromResult(expectedResult));
            //Act
            var actualResult = await _targetService.GetDetailsData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}