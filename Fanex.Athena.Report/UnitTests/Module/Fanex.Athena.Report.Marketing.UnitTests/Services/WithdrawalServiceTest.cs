using AutoFixture;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Services
{
    public class WithdrawalServiceTest
    {
        private readonly WithdrawalService _target;
        private readonly Mock<IWithdrawalRepository> _mockRepository;
        private readonly Mock<ICalculationService> _mockCalculationService;
        private readonly Fixture _fixture;

        public WithdrawalServiceTest()
        {
            _mockRepository = new Mock<IWithdrawalRepository>();
            _mockCalculationService = new Mock<ICalculationService>();
            _target = new WithdrawalService(_mockRepository.Object, _mockCalculationService.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetSummaryData_WhenCall_ChangeRateCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<WithdrawalSummaryMetaModel>();
            var fakeRawData = _fixture.Create<WithdrawalSummaryDTO>();
            var fakeWithdrawalAmountChangeRate = _fixture.Create<double>();
            var fakeWithdrawalUsersChangeRate = _fixture.Create<double>();

            _mockRepository.Setup(_ => _.GetSummaryData(metaModel)).Returns(Task.FromResult(fakeRawData));

            _mockCalculationService.Setup(_ => _.CalculateGrowth((double)fakeRawData.WithdrawalAmount, (double)fakeRawData.CompareWithdrawalAmount))
                .Returns(fakeWithdrawalAmountChangeRate);
            _mockCalculationService.Setup(_ => _.CalculateGrowth(fakeRawData.WithdrawalUsers, fakeRawData.CompareWithdrawalUsers))
                .Returns(fakeWithdrawalUsersChangeRate);

            //Act
            var actualResult = await _target.GetSummaryData(metaModel);

            //Assert
            Assert.Equal(fakeRawData, actualResult);
            Assert.Equal(fakeRawData.WithdrawalAmountChangeRate, fakeWithdrawalAmountChangeRate);
            Assert.Equal(fakeRawData.WithdrawalUsersChangeRate, fakeWithdrawalUsersChangeRate);
        }
    }
}