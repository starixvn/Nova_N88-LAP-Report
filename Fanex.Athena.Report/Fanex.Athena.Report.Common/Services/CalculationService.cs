using Fanex.Athena.Report.Common.Services.Interfaces;
using System;

namespace Fanex.Athena.Report.Common.Services
{
    public class CalculationService : ICalculationService
    {
        public double CalculateDistribution(double? value, double total)
        {
            value = value ?? 0;

            if (value.Value.CompareTo(0) == 0 || total.CompareTo(0) == 0)
            {
                return 0;
            }

            var result = (value.Value / Math.Abs(total)) * 100;

            return result;
        }

        public double CalculateGrowth(double? current, double? previous)
        {
            current = current ?? 0;
            previous = previous ?? 0;

            var baseValue = previous.Value;
            if (previous.Value.CompareTo(0) == 0)
            {
                baseValue = current.Value.CompareTo(0) == 0 ? 1 : current.Value;
            }

            var result = ((current.Value - previous.Value) / Math.Abs(baseValue)) * 100;

            return result;
        }

        public double ToPercentUnit(double? value)
        {
            value = value ?? 0;
            var result = value.Value * 100;

            return result;
        }

        public double ToRound(double? value, int numberOfRound = 2)
        {
            return Math.Round(value ?? 0, numberOfRound);
        }
    }
}