using AutoFixture;
using Fanex.Athena.Report.BonusInfo.MetaModels;
using Fanex.Athena.Report.BonusInfo.Repositories;
using Fanex.Data;
using Moq;
using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.BonusInfo.UnitTests.Repositories
{
    public class BonusInfoRepositoryTest : IDisposable
    {
        private readonly BonusInfoRepository _target;
        private readonly IFixture _fixture = new Fixture();

        public BonusInfoRepositoryTest()
        {
            DbSettingProviderManager.StartTestingSession();
            ObjectDbFactory.SwitchToTestingMode();
            _target = new BonusInfoRepository();
        }

        public void Dispose()
        {
            ObjectDbFactory.SwitchToNormalMode();
        }

        [Fact]
        public async Task GetSummaryData_WhenCall_CallCorrectSPAndParams()
        {
            //Arrange
            const string spKey = "N88_Rpt_BonusInfo";
            var metaModel = _fixture.Create<BonusInfoSummaryMetaModel>();
            var mockObjectDB = new Mock<IObjectDb>();
            MockObjectDB(spKey, mockObjectDB.Object);
            mockObjectDB.Setup(_ => _.ExecuteReaderAsync(It.IsAny<BonusInfoSummaryMetaModel>(), null, default))
                .Returns(Task.FromResult(Mock.Of<IDataReader>()));

            //Act
            _ = await _target.GetSummaryData(metaModel);

            //Assert
            mockObjectDB.Verify(_ => _.ExecuteReaderAsync(metaModel, null, default), Times.Once);
        }

        [Fact]
        public async Task GetDetailsData_WhenCall_CallCorrectSPAndParams()
        {
            //Arrange
            const string spKey = "N88_Rpt_BonusInfo_Detail";
            var metaModel = _fixture.Create<BonusInfoDetailMetaModel>();
            var mockObjectDB = new Mock<IObjectDb>();
            MockObjectDB(spKey, mockObjectDB.Object);
            mockObjectDB.Setup(_ => _.ExecuteReaderAsync(It.IsAny<BonusInfoDetailMetaModel>(), null, default))
                .Returns(Task.FromResult(Mock.Of<IDataReader>()));

            //Act
            _ = await _target.GetDetailsData(metaModel);

            //Assert
            mockObjectDB.Verify(_ => _.ExecuteReaderAsync(metaModel, null, default), Times.Once);
        }

        private void MockObjectDB(string key, IObjectDb objectDb)
        {
            ObjectDbFactory.Mock((_key, timeount) =>
            {
                if (key.Equals(_key))
                {
                    return objectDb;
                }
                return null;
            });
        }
    }
}