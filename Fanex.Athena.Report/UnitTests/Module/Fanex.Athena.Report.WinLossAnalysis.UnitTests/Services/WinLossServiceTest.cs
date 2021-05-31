using AutoFixture;
using Fanex.Athena.Report.WinLossAnalysis.DTO;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Repositories;
using Fanex.Athena.Report.WinLossAnalysis.Services;
using Fanex.Athena.Report.WinLossAnalysis.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Fanex.Athena.Report.Common.Model.DTO;

namespace Fanex.Athena.Report.WinLossAnalysis.UnitTests.Services
{
    public class WinLossServiceTest
    {
        private readonly WinLossService _target;
        private readonly Mock<IWinLossRepository> _mockRepository;
        private readonly Mock<IDataMapper> _mockDataMapper;
        private readonly Mock<ICurrencyService> _mockCurrencyService;
        private readonly IFixture _fixture = new Fixture();

        public WinLossServiceTest()
        {
            _mockRepository = new Mock<IWinLossRepository>();
            _mockDataMapper = new Mock<IDataMapper>();
            _mockCurrencyService = new Mock<ICurrencyService>();
            _target = new WinLossService(_mockRepository.Object, _mockCurrencyService.Object, _mockDataMapper.Object);
        }

        [Fact]
        public async Task GetData_EmptyResult_ReturnDefaultResult()
        {
            //Arrange
            var metaModel = _fixture.Create<WinLossMetaModel>();
            var data = new Tuple<IEnumerable<WinLossDataItemDTO>, WinLossDataTotalDTO, int>(new List<WinLossDataItemDTO>(), null, 0);
            _mockRepository.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult((data.Item1, data.Item2, data.Item3)));

            //Act
            var result = await _target.GetData(metaModel);

            //Assert
            result.Should().BeEquivalentTo(SortableResultModel<WinLossRecordViewModel>.Default);
        }

        [Fact]
        public async Task GetData_SuccessfulSameTotalData_ReturnResult()
        {
            //Arrange
            var metaModel = _fixture.Create<WinLossMetaModel>();
            WinLossDataItemDTO data = new WinLossDataItemDTO() { Group = "g1", CurrencyId = 1, CurrencyCode = "1", CustId = 1, SabaId = "123", SabaSystemId = "111", BetCount = 1000 };
            WinLossDataTotalDTO dataTotal = new WinLossDataTotalDTO() {
                TotalBasedTurnoverNoDraw = 0,
                TotalBasedGGR = 0,
                TotalBetCount = 0,
                TotalBasedAvgBetAmount = 0,
                TotalUniquePlayers = 0
            };
            List<WinLossDataItemDTO> listData = new List<WinLossDataItemDTO>() { data };
            var dataExpected = new Tuple<IEnumerable<WinLossDataItemDTO>, WinLossDataTotalDTO, int>(listData, dataTotal, 1);

            WinLossRecordViewModel dataTotalExpected = new WinLossRecordViewModel()
            {
                Turnover = 0,
                Ggr = 0,
                BetCount = 0,
                AvgBetAmount = 0,
                UniquePlayers = 0,
                Margin = 0
            };

            _mockRepository.Setup(_ => _.GetData(metaModel)).Returns(Task.FromResult((dataExpected.Item1, dataExpected.Item2, dataExpected.Item3)));
            IDictionary<int, Currency> currencies = new Dictionary<int, Currency>();
            currencies.Add(1, new Currency() { ID = 1, Name = "USD" });
            _mockCurrencyService.Setup(_ => _.GetCurrencyDictionaryAsync()).Returns(Task.FromResult(currencies));

            //Act
            var result = await _target.GetData(metaModel);

            //Assert
            dataTotalExpected.Should().BeEquivalentTo(result.TotalData);
            Assert.Equal(1, result.TotalRows);
        }
    }
}