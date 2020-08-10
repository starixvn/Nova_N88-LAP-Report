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
    public class UserActivitiesControllerTest
    {
        private readonly Mock<IUserActivitiesService> _mockService;
        private readonly UserActivitiesController _targetController;
        private readonly IFixture _fixture = new Fixture();

        public UserActivitiesControllerTest()
        {
            _mockService = new Mock<IUserActivitiesService>();
            _targetController = new UserActivitiesController(_mockService.Object);
        }

        [Fact]
        public async Task GetData_WhenCall_CallMetaModelCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<UserActivitiesMetaModel>();
            var expectedResult = _fixture.Create<IEnumerable<UserActivitiesViewModel>>();

            _mockService.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult(expectedResult));

            //Act
            var actualResult = await _targetController.GetData(metaModel);

            //Assert
            expectedResult.Should().BeEquivalentTo(actualResult.Value as IEnumerable<UserActivitiesViewModel>);
        }
    }
}