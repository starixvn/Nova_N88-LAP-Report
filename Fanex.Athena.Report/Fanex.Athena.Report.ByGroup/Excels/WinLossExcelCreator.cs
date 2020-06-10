using Fanex.Athena.Report.ByGroup.Enums;
using Microsoft.AspNetCore.Http;

namespace Fanex.Athena.Report.ByGroup.Excels
{
    public class WinLossExcelCreator : IWinLossExcelCreator
    {
        protected readonly IHttpContextAccessor _contextAccessor;

        public WinLossExcelCreator(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public IWinLossExcel CreateWinLossExcel(byte group)
        {
            var serviceProvider = _contextAccessor.HttpContext.RequestServices;
            if (group == (byte)GroupBy.Player)
            {
                return serviceProvider.GetService(typeof(WinLossPlayerExcel)) as IWinLossExcel;
            }

            return serviceProvider.GetService(typeof(WinLossExcel)) as IWinLossExcel;
        }
    }
}