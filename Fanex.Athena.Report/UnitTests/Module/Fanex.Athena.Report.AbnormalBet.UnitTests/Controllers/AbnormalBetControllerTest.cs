using AutoFixture;
using Fanex.Athena.Report.AbnormalBet.Consts;
using Fanex.Athena.Report.AbnormalBet.Controllers;
using Fanex.Athena.Report.AbnormalBet.DTO;
using Fanex.Athena.Report.AbnormalBet.MetaModels;
using Fanex.Athena.Report.AbnormalBet.Services.Interfaces;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Moq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.BonusInfo.UnitTests.Controllers
{
    public class AbnormalBetControllerTest
    {
        private readonly Mock<IAbnormalBetService> _mockService;
        private readonly AbnormalBetController _targetController;
        private readonly IFixture _fixture = new Fixture();

        public AbnormalBetControllerTest()
        {
            _mockService = new Mock<IAbnormalBetService>();
            _targetController = new AbnormalBetController(_mockService.Object);
        }

        [Fact]
        public async Task GetData_WhenCallGetSummaryData_ReturnDataCorrectly()
        {
            //Arrange
            var metaModel = new AbnormalBetMetaModel { ViewIn = (int)AbnormalBetViewInLevel.Summary };
            var expectedResult = _fixture.Create<SortableResultModel<AbnormalBetSummary>>();

            _mockService.Setup(_ => _.GetDataAsync(metaModel)).ReturnsAsync(expectedResult);

            //Act
            var actualResult = await _targetController.GetData(metaModel);

            //Assert
            Assert.NotNull(JsonConvert.SerializeObject(actualResult.Value));
        }
    }
}