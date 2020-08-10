using AutoFixture;
using Fanex.Athena.Report.Marketing.Controllers;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Services;
using Fanex.Athena.Report.Marketing.ViewModels;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Controllers
{
    public class RevenueControllerTest
    {
        private readonly Mock<IRevenueService> _mockService;
        private readonly RevenueController _targetController;
        private readonly IFixture _fixture = new Fixture();

        public RevenueControllerTest()
        {
            _mockService = new Mock<IRevenueService>();
            _targetController = new RevenueController(_mockService.Object);
        }

        [Fact]
        public async Task GetData_WhenCall_CallMetaModelCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<AvgRevenueMetaModel>();
            var expectedResult = _fixture.Create<IEnumerable<AvgRevenueViewModel>>();

            _mockService.Setup(_ => _.GetAvgData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _targetController.GetAvgData(metaModel);

            //Assert
            expectedResult.Should().BeEquivalentTo(actualResult.Value as IEnumerable<AvgRevenueViewModel>);
        }
    }
}