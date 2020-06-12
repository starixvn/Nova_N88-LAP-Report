namespace Fanex.Athena.Report.Common.Helpers
{
    public static class CalculatorHepler
    {
        private const decimal Zero = 0.0m;

        public static double Margin(decimal profit, decimal turnover)
        {
            if (turnover == Zero)
            {
                return 0.0;
            }
            return Percent((double)(profit / turnover));
        }

        public static double Percent(double value)
        {
            const double percent = 100.00;
            return value * percent;
        }
    }
}