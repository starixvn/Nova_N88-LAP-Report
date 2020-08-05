using AutoFixture;
using Fanex.Athena.Report.Common.Services;
using System;
using Xunit;

namespace Fanex.Athena.Report.Common.UnitTests.Services
{
    public class CalculationServiceTest
    {
        private readonly CalculationService _targetService;
        private readonly Fixture _fixture;

        public CalculationServiceTest()
        {
            _targetService = new CalculationService();
            _fixture = new Fixture();
        }

        [Fact]
        public void CalculateDistribution_TotalIsZero_ReturnZero()
        {
            //Arrange
            var value = _fixture.Create<double>();

            //Act
            var actual = _targetService.CalculateDistribution(value, 0);

            //Assert
            Assert.Equal(0, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(null)]
        public void CalculateDistribution_ValueIsNullOrZero_ReturnZero(double? value)
        {
            //Arrange
            //Act
            var actual = _targetService.CalculateDistribution(value, 0);

            //Assert
            Assert.Equal(0, actual);
        }


        [Theory]
        [InlineData(0, 1.25, 0)]
        [InlineData(15, 100, 15)]
        [InlineData(10.55, 98.22, 10.74)]
        public void CalculateDistribution_WhenCall_ReturnCorrectResult(double value, double total, double expected)
        {
            //Arrange

            //Act
            var actual = _targetService.CalculateDistribution(value, total);

            //Assert
            Assert.Equal(expected, Math.Round(actual, 2));
        }


        [Theory]
        [InlineData(null, 0, 0)]
        [InlineData(0, null, 0)]
        [InlineData(null, null, 0)]
        [InlineData(-123, null, -100)]
        [InlineData(1235, 0, 100)]
        public void CalculateGrowth_PreviousIsNullOrZero_ReturnCorrectResult(double? current, double? previous, double expected)
        {
            //Arrange
            //Act
            var actual = _targetService.CalculateGrowth(current, previous);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, -123.123, 100)]
        [InlineData(0, 123.123, -100)]
        [InlineData(null, -123.123, 100)]
        [InlineData(null, 123.123, -100)]
        public void CalculateGrowth_CurrentIsNullOrZero_ReturnCorrectResult(double? current, double? previous, double expected)
        {
            //Arrange
            //Act
            var actual = _targetService.CalculateGrowth(current, previous);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1.23, 1.23, 0)]
        [InlineData(1, 100, -99)]
        [InlineData(1.5, 100.23, -98.5)]
        [InlineData(-50, -100, 50)]
        [InlineData(100, 50, 100)]
        public void CalculateGrowth_NotNullAndNotZero_ReturnCorrectResult(double? current, double? previous, double expected)
        {
            //Arrange

            //Act
            var actual = _targetService.CalculateGrowth(current, previous);

            //Assert
            Assert.Equal(expected, Math.Round(actual, 2));
        }

        [Fact]
        public void ToPercentUnit_ValueIsNull_ReturnZero()
        {
            //Arrange

            //Act
            var actual = _targetService.ToPercentUnit(null);

            //Assert
            Assert.Equal(0, actual);
        }


        [Theory]
        [InlineData(0.86732, 86.73)]
        [InlineData(0.001, 0.1)]
        [InlineData(0.91235, 91.24)]
        public void ToPercentUnit_WhenCall_ReturnCorrectResult(double value, double expected)
        {
            //Arrange

            //Act
            var actual = _targetService.ToPercentUnit(value);

            //Assert
            Assert.Equal(expected, Math.Round(actual, 2));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(null, 0)]
        [InlineData(1.246, 1.25)]
        [InlineData(1.256, 1.26)]
        [InlineData(1.266, 1.27)]
        public void ToRound_WhenCall_ReturnCorrectResult(double? value, double expected)
        {
            //Arrange
            //Act
            var actual = _targetService.ToRound(value);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}