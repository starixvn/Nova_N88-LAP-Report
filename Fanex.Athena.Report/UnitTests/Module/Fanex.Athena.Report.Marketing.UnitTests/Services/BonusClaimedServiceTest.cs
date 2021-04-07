using AutoFixture;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Services
{
    public class BonusClaimedServiceTest
    {
        private readonly BonusClaimedService _target;
        private readonly Mock<IBonusClaimedRepository> _mockRepository;
        private readonly Mock<ICalculationService> _mockCalculationService;
        private readonly Mock<IDataMapper> _mockDataMapper;
        private readonly Mock<ICurrencyService> _mockCurrencyService;
        private readonly Fixture _fixture;

        public BonusClaimedServiceTest()
        {
            _mockRepository = new Mock<IBonusClaimedRepository>();
            _mockCalculationService = new Mock<ICalculationService>();
            _mockDataMapper = new Mock<IDataMapper>();
            _mockCurrencyService = new Mock<ICurrencyService>();
            _target = new BonusClaimedService(_mockRepository.Object, _mockCalculationService.Object, _mockCurrencyService.Object, _mockDataMapper.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetSummaryData_WhenCall_ChangeRateCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<BonusClaimedSummaryMetaModel>();
            var fakeTotalData = _fixture.Create<TotalBonusClaimedDTO>();
            var fakeRawData = new BonusClaimedSummaryDTO
            {
                TotalData = new List<TotalBonusClaimedDTO> { fakeTotalData },
                DetailData = new List<DetailBonusClaimedDTO>
                {
                    new DetailBonusClaimedDTO
                    {
                        BonusAmount = 10.2m,
                        NumberOfBonusClaimed = 5
                    },
                    new DetailBonusClaimedDTO
                    {
                        BonusAmount = 5.8m,
                        NumberOfBonusClaimed = 15
                    },
                    new DetailBonusClaimedDTO
                    {
                        BonusAmount = 4,
                        NumberOfBonusClaimed = 5
                    }
                }
            };
            fakeRawData.TotalData = new List<TotalBonusClaimedDTO> { fakeTotalData };
            var fakeTotalDataChangeRate = _fixture.Create<double>();

            _mockRepository.Setup(_ => _.GetSummaryData(metaModel)).Returns(Task.FromResult(fakeRawData));

            _mockCalculationService.Setup(_ => _.CalculateGrowth((double)fakeTotalData.BonusAmountClaimeds, (double)fakeTotalData.CompareBonusAmountClaimeds))
                .Returns(fakeTotalDataChangeRate);

            //Act
            var actualResult = await _target.GetSummaryData(metaModel);

            //Assert
            Assert.Equal(fakeRawData, actualResult);
            Assert.Equal(fakeRawData.TotalData.FirstOrDefault().ChangeRate, fakeTotalDataChangeRate);
            Assert.Equal(20.00, (double)fakeRawData.TotalDetailData.TotalBonusAmount);
            Assert.Equal(25, fakeRawData.TotalDetailData.TotalBonusClaimeds);
        }
    }
}