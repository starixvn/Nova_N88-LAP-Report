using AutoFixture;
using Fanex.Athena.Report.WinLossAnalysis.MetaModels;
using Fanex.Athena.Report.WinLossAnalysis.Repositories;
using Fanex.Data;
using Moq;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.WinLossAnalysis.UnitTests.Repositories
{
    public class WinLossRepositoryTest : IDisposable
    {
        private readonly WinLossRepository _target;
        private readonly IFixture _fixture = new Fixture();

        public WinLossRepositoryTest()
        {
            DbSettingProviderManager.StartTestingSession();
            ObjectDbFactory.SwitchToTestingMode();
            _target = new WinLossRepository();
        }

        public void Dispose()
        {
            ObjectDbFactory.SwitchToNormalMode();
        }

        [Fact]
        public async Task GetData_WhenCall_CallCorrectSPAndParams()
        {
            //Arrange
            const string spKey = "N88_Rpt_Winloss";
            var metaModel = _fixture.Create<WinLossMetaModel>();
            var mockObjectDB = new Mock<IObjectDb>();
            MockObjectDB(spKey, mockObjectDB.Object);
            mockObjectDB.Setup(_ => _.ExecuteReaderAsync(It.IsAny<WinLossMetaModel>(), null, default(CancellationToken)))
                .Returns(Task.FromResult(Mock.Of<IDataReader>()));

            //Act
            _ = await _target.GetData(metaModel);

            //Assert
            mockObjectDB.Verify(_ => _.ExecuteReaderAsync(metaModel, null, default(CancellationToken)), Times.Once);
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