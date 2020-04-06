namespace Fanex.Athena.Report.Resources.Helpers
{
    public static class DataHelper
    {
        public static string GetProductName(string productCode)
        {
            return ViewResources.ResourceManager.GetString(productCode);
        }
    }
}