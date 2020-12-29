using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.Common.Excel
{
    public static class ExcelHelper
    {
        private const string EmptySympol = "-";

        public static void SetTextRowHeader(IRow row, IEnumerable<string> headerTextList)
        {
            for (int i = 0; i < headerTextList.Count(); i++)
            {
                row.Cells[i].SetCellValue(headerTextList.ElementAt(i));
            }
        }

        public static void SetValueCellDecimal(ICell cell, decimal? value, bool allowedZero = true, string emptySympol = EmptySympol)
        {
            if (!value.HasValue)
            {
                cell.SetCellValue(string.Empty);
            }
            else if (!allowedZero && value == 0)
            {
                cell.SetCellValue(emptySympol);
            }
            else
            {
                cell.SetCellValue((double)value);
            }
        }

        public static int GetNumberOfSheets(int totalRows, int pageSize)
        {
            return totalRows % pageSize == 0 ? totalRows / pageSize : (totalRows / pageSize) + 1;
        }
    }
}