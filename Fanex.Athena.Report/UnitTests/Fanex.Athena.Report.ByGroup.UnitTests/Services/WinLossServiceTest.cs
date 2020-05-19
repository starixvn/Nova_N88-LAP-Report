using AutoFixture;
using Fanex.Athena.Report.ByGroup.DTO;
using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.ByGroup.Repositories;
using Fanex.Athena.Report.ByGroup.Services;
using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Fanex.Athena.Report.ByGroup.UnitTests.Services
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
    }
}