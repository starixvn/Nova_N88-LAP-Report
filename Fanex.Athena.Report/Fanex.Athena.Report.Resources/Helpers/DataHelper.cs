namespace Fanex.Athena.Report.Resources.Helpers
{
    public static class DataHelper
    {
        public static string GetProductName(string productCode)
        {
            return GetValue(productCode);
        }

        public static string GetValue(string key)
        {
            return ViewResources.ResourceManager.GetString(key);
        }
    }
}