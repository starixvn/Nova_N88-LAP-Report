using AutoFixture;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using Fanex.Athena.Report.Marketing.ViewModels;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Controllers
{
    public class PerformanceHighlightControllerTest
    {
        private readonly Mock<IPerformanceHighlightService> _mockService;
        private readonly PerformanceHighlightController _targetController;
        private readonly IFixture _fixture = new Fixture();

        public PerformanceHighlightControllerTest()
        {
            _mockService = new Mock<IPerformanceHighlightService>();
            _targetController = new PerformanceHighlightController(_mockService.Object);
        }

        [Fact]
        public async Task GetData_WhenCall_CallMetaModelCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<PerformanceHighlightMetaModel>();
            var expectedResult = _fixture.Create<PerformanceHighlightViewModel>();

            _mockService.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _targetController.GetData(metaModel);

            //Assert
            Assert.Equal(expectedResult, actualResult.Value as PerformanceHighlightViewModel);
        }
    }
}