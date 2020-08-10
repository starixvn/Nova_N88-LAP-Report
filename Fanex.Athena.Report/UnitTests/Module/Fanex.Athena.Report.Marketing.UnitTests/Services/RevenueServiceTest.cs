using AutoFixture;
using Fanex.Athena.Report.Common.Model.DTO;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using Fanex.Athena.Report.Marketing.ViewModels;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Services
{
    public class RevenueServiceTest
    {
        private readonly RevenueService _targetService;
        private readonly Mock<IRevenueRepository> _mockRepository;
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<IDataMapper> _mockDataMapper;
        private readonly IFixture _fixture = new Fixture();

        public RevenueServiceTest()
        {
            _mockRepository = new Mock<IRevenueRepository>();
            _mockProductService = new Mock<IProductService>();
            _mockDataMapper = new Mock<IDataMapper>();
            _targetService = new RevenueService(_mockRepository.Object, _mockProductService.Object, _mockDataMapper.Object);
        }

        [Fact]
        public async Task GetAvgData_WhenCall_ReturnDataCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<AvgRevenueMetaModel>();
            var fakeRawData = new List<AvgRevenueDTO>
            {
                new AvgRevenueDTO
                {
                    ProductId = 1,
                    Turnover = 100.12M,
                    PaidUserTurnover = 200.54M
                },
                new AvgRevenueDTO
                {
                    ProductId = 2,
                    Turnover = 1235.123M,
                    PaidUserTurnover = 988
                }
            };

            var fakeProducts = _fixture.Create<IDictionary<int, TpProduct>>();

            var expectedResult = new List<AvgRevenueViewModel>
            {
                new AvgRevenueViewModel
                {
                    ProductId = 1,
                    ProductName = "Product1",
                    AllUsersRevenue = 100.12M,
                    PaidUsersRevenue =  200.54M
                },
                new AvgRevenueViewModel
                {
                    ProductId = 2,
                    ProductName = "Product2",
                    AllUsersRevenue =  1235.123M,
                    PaidUsersRevenue = 988
                }
            };

            _mockRepository.Setup(_ => _.GetAvgData(metaModel)).Returns(Task.FromResult<IEnumerable<AvgRevenueDTO>>(fakeRawData));
            _mockProductService.Setup(_ => _.GetDictionaryTpProductAsync()).Returns(Task.FromResult(fakeProducts));
            _mockDataMapper.Setup(_ => _.ToTpProductName(fakeProducts, 1)).Returns("Product1");
            _mockDataMapper.Setup(_ => _.ToTpProductName(fakeProducts, 2)).Returns("Product2");

            //Act
            var actualResult = await _targetService.GetAvgData(metaModel);

            //Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}