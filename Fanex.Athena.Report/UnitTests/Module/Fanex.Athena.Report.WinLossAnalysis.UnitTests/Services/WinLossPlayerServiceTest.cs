using AutoFixture;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Repositories;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.WinLossAnalysis.UnitTests.Services
{
    public class WinLossPlayerServiceTest
    {
        private readonly WinLossPlayerService _target;
        private readonly Mock<IWinLossPlayerRepository> _mockRepository;
        private readonly IFixture _fixture = new Fixture();

        public WinLossPlayerServiceTest()
        {
            _mockRepository = new Mock<IWinLossPlayerRepository>();
            _target = new WinLossPlayerService(_mockRepository.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetPlayerDetai_WhenCall_ReturnDataCorrectly(bool drilldownInOriginalCurrency)
        {
            //Arrange
            var metaModel = _fixture.Create<WinLossPlayerDetailMetaModel>();
            metaModel.DrilldownInOriginalCurrency = drilldownInOriginalCurrency;

            var rawDataItem = _fixture.Create<WinLossPlayerDetailDTO>();
            var rawData = new List<WinLossPlayerDetailDTO> { rawDataItem };

            _mockRepository.Setup(_ => _.GetPlayerDetail(metaModel)).Returns(Task.FromResult<IEnumerable<WinLossPlayerDetailDTO>>(rawData));

            //Act
            var result = await _target.GetPlayerDetail(metaModel);

            //Assert
            var actualResult = result.First();

            Assert.Equal(actualResult.Key, rawDataItem.CustId + "_" + rawDataItem.ProductId);
            Assert.Equal(actualResult.ParentName, metaModel.UsernameParent);
            Assert.Equal(actualResult.OriginalCurrency, metaModel.OriginalCurrency);
            Assert.Equal(actualResult.Turnover, drilldownInOriginalCurrency ? rawDataItem.Turnover : rawDataItem.BasedTurnover);
            Assert.Equal(actualResult.NetTurnover, drilldownInOriginalCurrency ? rawDataItem.NetTurnover : rawDataItem.BasedNetTurnover);
            Assert.Equal(actualResult.Ggr, drilldownInOriginalCurrency ? rawDataItem.GGR : rawDataItem.BasedGGR);
            Assert.Equal(actualResult.PlayerWinloss, drilldownInOriginalCurrency ? rawDataItem.PlayerWinloss : rawDataItem.BasedPlayerWinloss);
            Assert.Equal(actualResult.AvgBetAmount, drilldownInOriginalCurrency ? rawDataItem.AvgBetAmount : rawDataItem.BasedAvgBetAmount);
        }
    }
}