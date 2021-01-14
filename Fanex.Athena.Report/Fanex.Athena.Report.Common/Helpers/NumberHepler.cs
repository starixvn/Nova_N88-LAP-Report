namespace Fanex.Athena.Report.Common.Helpers
{
    public static class NumberHepler
    {
        private const decimal Zero = 0.0m;
        private const string DecimalFormat = "{0:#,##0.##}";

        public static double Margin(decimal profit, decimal turnover)
        {
            if (turnover == Zero)
            {
                return 0.0;
            }
            return Percent((double)(profit / turnover));
        }

        public static decimal Divide(decimal divisor, int dividend)
        {
            if (dividend == Zero)
            {
                return Zero;
            }
            return divisor / dividend;
        }

        private static double Percent(double value)
        {
            const double percent = 100.00;
            return value * percent;
        }

        public static string Format(decimal? number)
        {
            if (!number.HasValue)
            {
                return string.Empty;
            }
            return string.Format(DecimalFormat, number.Value);
        }
    }
}