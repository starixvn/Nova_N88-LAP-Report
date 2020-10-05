using AutoFixture;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using Fanex.Athena.Report.Marketing.ViewModels;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Services
{
    public class PerformanceHighlightServiceTest
    {
        private readonly PerformanceHighlightService _targetService;
        private readonly Mock<IPerformanceHighlightRepository> _mockRepository;
        private readonly Mock<ICalculationService> _mockCalculationService;
        private readonly IFixture _fixture = new Fixture();

        public PerformanceHighlightServiceTest()
        {
            _mockRepository = new Mock<IPerformanceHighlightRepository>();
            _mockCalculationService = new Mock<ICalculationService>();
            _targetService = new PerformanceHighlightService(_mockRepository.Object, _mockCalculationService.Object);
        }

        [Fact]
        public async Task GetData_WhenCall_ReturnDataCorrectly()
        {
            //Arrange
            var rawData = new PerformanceHighlightDTO
            {
                TotalUsers = 11384,
                UptoTotalUsers = 123,
                CompareUptoTotalUsers = 10248,
                TotalDepositUsers = 109,
                NewlyRegisterUsers = 1101,
                CompareNewlyRegisterUsers = 2015,
                ActiveUsers = 541,
                CompareActiveUsers = 153
            };

            var expectedResult = new PerformanceHighlightViewModel
            {
                TotalUsers = 11384,
                TotalDepositUsers = 109,
                NewlyRegisterUsers = 1101,
                GrowthRateNewlyRegisterUsers = 45,
                ActiveUsers = 541,
                GrowthRateActiveUsers = 254,
                ChurnRate = 95,
                RetentionRate = 5,
                DifferentChurnRate = -3,
                DifferentRetentionRate = 3
            };

            var metaModel = _fixture.Create<PerformanceHighlightMetaModel>();
            _mockRepository.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(rawData));

            _mockCalculationService.Setup(_ => _.CalculateGrowth(rawData.ActiveUsers, rawData.CompareActiveUsers)).Returns(expectedResult.GrowthRateActiveUsers);
            _mockCalculationService.Setup(_ => _.CalculateGrowth(rawData.NewlyRegisterUsers, rawData.CompareNewlyRegisterUsers)).Returns(expectedResult.GrowthRateNewlyRegisterUsers);

            _mockCalculationService.Setup(_ => _.CalculateDistribution(rawData.UptoTotalUsers - rawData.ActiveUsers, rawData.UptoTotalUsers)).Returns(expectedResult.ChurnRate);
            _mockCalculationService.Setup(_ => _.CalculateDistribution(rawData.CompareUptoTotalUsers - rawData.CompareActiveUsers, rawData.CompareUptoTotalUsers)).Returns(expectedResult.ChurnRate - expectedResult.DifferentChurnRate);

            //Act
            var actualResult = await _targetService.GetData(metaModel);

            //Assert
            expectedResult.Should().BeEquivalentTo(actualResult);
        }
    }
}