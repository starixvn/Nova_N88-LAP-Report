using AutoFixture;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Common.Services.Interfaces;
using Fanex.Athena.Report.Marketing.DTO;
using Fanex.Athena.Report.Marketing.MetaModels;
using Fanex.Athena.Report.Marketing.Repositories;
using Fanex.Athena.Report.Marketing.Services;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.Report.Marketing.UnitTests.Services
{
    public class NewlyRegisterUsersServiceTest
    {
        private readonly NewlyRegisterUsersService _targetService;
        private readonly Mock<INewlyRegisterUsersRepository> _mockRepository;
        private readonly Mock<ICalculationService> _mockCalculationService;
        private readonly IFixture _fixture = new Fixture();

        public NewlyRegisterUsersServiceTest()
        {
            _mockRepository = new Mock<INewlyRegisterUsersRepository>();
            _mockCalculationService = new Mock<ICalculationService>();
            _targetService = new NewlyRegisterUsersService(_mockRepository.Object, _mockCalculationService.Object);
        }

        [Fact]
        public async Task GetSummaryData_WhenCall_ReturnDataCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<NewlyRegisterUsersSummaryMetaModel>();
            var fakeTotalData = _fixture.Create<TotalNewlyRegisterUsersDTO>();
            var fakeTotalRate = _fixture.Create<double>();
            var expectedResult = new NewlyRegisterUsersSummaryDTO
            {
                TotalData = new List<TotalNewlyRegisterUsersDTO>
                {
                    fakeTotalData
                },
                CampaignData = _fixture.Create<IEnumerable<CampaignNewlyRegisterUsersDTO>>(),
                SignupData = new List<SignupUrlNewlyRegisterUsersDTO>
                {
                    new SignupUrlNewlyRegisterUsersDTO
                    {
                        SignUpSource = "AFF",
                        SignupUrl = "AFF",
                        DepositUsers = _fixture.Create<int>(),
                        NewlyRegisterUsers = _fixture.Create<int>(),
                        FirstDepositAmount = _fixture.Create<decimal>()
                    },
                    new SignupUrlNewlyRegisterUsersDTO
                    {
                        SignUpSource = "Credit Site",
                        SignupUrl = "Credit Site",
                        DepositUsers = _fixture.Create<int>(),
                        NewlyRegisterUsers = _fixture.Create<int>(),
                        FirstDepositAmount = _fixture.Create<decimal>()
                    },
                    new SignupUrlNewlyRegisterUsersDTO
                    {
                        SignUpSource = "None",
                        SignupUrl =  "None",
                        DepositUsers = _fixture.Create<int>(),
                        NewlyRegisterUsers = _fixture.Create<int>(),
                        FirstDepositAmount = _fixture.Create<decimal>()
                    },
                    new SignupUrlNewlyRegisterUsersDTO
                    {
                        SignUpSource = "Nova88.net",
                        SignupUrl = "nova88.com",
                        DepositUsers = _fixture.Create<int>(),
                        NewlyRegisterUsers = _fixture.Create<int>(),
                        FirstDepositAmount = _fixture.Create<decimal>()
                    },
                    new SignupUrlNewlyRegisterUsersDTO
                    {
                        SignUpSource = "Nova88.net",
                        SignupUrl = "nova88.net",
                        DepositUsers = _fixture.Create<int>(),
                        NewlyRegisterUsers = _fixture.Create<int>(),
                        FirstDepositAmount = _fixture.Create<decimal>()
                    },
                    new SignupUrlNewlyRegisterUsersDTO
                    {
                        SignUpSource = "Nova88.net",
                        SignupUrl = "m.nova88.net",
                        DepositUsers = _fixture.Create<int>(),
                        NewlyRegisterUsers = _fixture.Create<int>(),
                        FirstDepositAmount = _fixture.Create<decimal>()
                    }
                },

            };

            _mockRepository.Setup(_ => _.GetSummaryData(metaModel)).Returns(Task.FromResult(expectedResult));
            _mockCalculationService.Setup(_ => _.CalculateGrowth(fakeTotalData.NewlyRegisterUsers, fakeTotalData.CompareNewlyRegisterUsers)).Returns(fakeTotalRate);

            //Act
            var actualResult = await _targetService.GetSummaryData(metaModel);

            //Assert
            Assert.Equal(fakeTotalRate, actualResult.TotalData.FirstOrDefault()?.ChangeRate);
            Assert.Equal(expectedResult, actualResult);

            Assert.Equal(expectedResult.SignupData.Sum(_ => _.DepositUsers), actualResult.TotalSignupData.TotalDepositUsers);
            Assert.Equal(expectedResult.SignupData.Sum(_ => _.NewlyRegisterUsers), actualResult.TotalSignupData.TotalNewlyRegisterUsers);
            Assert.Equal(expectedResult.SignupData.Sum(_ => _.FirstDepositAmount), actualResult.TotalSignupData.TotalFirstDepositAmount);

            var affSignup = actualResult.SignupData.FirstOrDefault(_ => _.SignUpSource.Equals("AFF"));
            Assert.True(affSignup.NoGrouping);

            var creditSignup = actualResult.SignupData.FirstOrDefault(_ => _.SignUpSource.Equals("Credit Site"));
            Assert.True(creditSignup.NoGrouping);

            var noneSignup = actualResult.SignupData.FirstOrDefault(_ => _.SignUpSource.Equals("None"));
            Assert.True(noneSignup.NoGrouping);

            var mainSignup = actualResult.SignupData.FirstOrDefault(_ => _.SignUpSource.Equals("Nova88.net"));
            Assert.False(mainSignup.NoGrouping);
            Assert.Equal(3, mainSignup.Childs.Count());
        }

        [Fact]
        public async Task GetDetailsData_WhenCall_ReturnDataCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<NewlyRegisterUsersDetailMetaModel>();
            var expectedResult = new ResultModel<NewlyRegisterUsersDetailDTO>
            {
                DataList = _fixture.Create<IEnumerable<NewlyRegisterUsersDetailDTO>>(),
                TotalRows = _fixture.Create<int>()
            };

            _mockRepository.Setup(_ => _.GetDetailsData(metaModel)).Returns(Task.FromResult((expectedResult.DataList, expectedResult.TotalRows)));

            //Act
            var actualResult = await _targetService.GetDetailsData(metaModel);

            //Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetExportData_WhenCall_CallMetaModelCorrectly()
        {
            //Arrange
            var metaModel = _fixture.Create<NewlyRegisterUsersExportMetaModel>();
            var actualSummaryMetaModel = new NewlyRegisterUsersSummaryMetaModel();
            var actualDetailsMetaModel = new NewlyRegisterUsersDetailMetaModel();

            var expectedSummaryMetaModel = new NewlyRegisterUsersSummaryMetaModel
            {
                BrandId = metaModel.BrandId,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                CompareFromDate = metaModel.CompareFromDate,
                CompareToDate = metaModel.CompareToDate,
                BaseCurrency = metaModel.BaseCurrency
            };

            var expectedDetailMetaModel = new NewlyRegisterUsersDetailMetaModel
            {
                BrandId = metaModel.BrandId,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                BaseCurrency = metaModel.BaseCurrency,
                SortbyId = metaModel.SortbyId,
                IsSortAsc = metaModel.IsSortAsc,
                PageIndex = 1,
                PageSize = int.MaxValue - 100
            };


            _mockRepository.Setup(_ => _.GetSummaryData(It.IsAny<NewlyRegisterUsersSummaryMetaModel>()))
                .Returns(Task.FromResult(FakeSummaryDTO()))
                .Callback<NewlyRegisterUsersSummaryMetaModel>(model => actualSummaryMetaModel = model);

            _mockRepository.Setup(_ => _.GetDetailsData(It.IsAny<NewlyRegisterUsersDetailMetaModel>()))
                .Returns(Task.FromResult((_fixture.Create<IEnumerable<NewlyRegisterUsersDetailDTO>>(), _fixture.Create<int>())))
                .Callback<NewlyRegisterUsersDetailMetaModel>(model => actualDetailsMetaModel = model);

            //Act
            _ = await _targetService.GetExportData(metaModel);

            //Assert
            actualSummaryMetaModel.Should().BeEquivalentTo(expectedSummaryMetaModel);
            actualDetailsMetaModel.Should().BeEquivalentTo(expectedDetailMetaModel);
        }

        [Fact]
        public async Task GetExportData_WhenCall_ReturnDataCorrectly1()
        {
            //Arrange
            var metaModel = _fixture.Create<NewlyRegisterUsersExportMetaModel>();
            var expectedSummaryData = FakeSummaryDTO();
            var expectedDetailResult = new ResultModel<NewlyRegisterUsersDetailDTO>
            {
                DataList = _fixture.Create<IEnumerable<NewlyRegisterUsersDetailDTO>>(),
                TotalRows = _fixture.Create<int>()
            };

            _mockRepository.Setup(_ => _.GetSummaryData(It.IsAny<NewlyRegisterUsersSummaryMetaModel>())).Returns(Task.FromResult(expectedSummaryData));

            _mockRepository.Setup(_ => _.GetDetailsData(It.IsAny<NewlyRegisterUsersDetailMetaModel>())).Returns(Task.FromResult((expectedDetailResult.DataList, expectedDetailResult.TotalRows)));

            //Act
            var (actualSummaryResult, actualDetailResult) = await _targetService.GetExportData(metaModel);

            //Assert
            actualSummaryResult.Should().BeEquivalentTo(expectedSummaryData);
            actualDetailResult.Should().BeEquivalentTo(expectedDetailResult);
        }

        private NewlyRegisterUsersSummaryDTO FakeSummaryDTO()
        {
            return new NewlyRegisterUsersSummaryDTO
            {
                TotalData = _fixture.CreateMany<TotalNewlyRegisterUsersDTO>(),
                CampaignData = _fixture.CreateMany<CampaignNewlyRegisterUsersDTO>(),
                TotalSignupData = _fixture.Create<TotalSignupUrlNewlyRegisterUsersDTO>(),
                SignupData = new List<SignupUrlNewlyRegisterUsersDTO>()
            };
        }
    }
}