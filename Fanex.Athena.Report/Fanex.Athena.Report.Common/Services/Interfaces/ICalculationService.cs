namespace Fanex.Athena.Report.Common.Services.Interfaces
{
    public interface ICalculationService
    {
        double CalculateGrowth(double? current, double? previous);

        double CalculateDistribution(double? value, double total);

        double ToPercentUnit(double? value);

        double ToRound(double? value, int numberOfRound = 2);
    }
}