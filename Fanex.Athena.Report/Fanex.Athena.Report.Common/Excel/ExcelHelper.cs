using NPOI.SS.UserModel;
using System;
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

        public static void SetCellValue(this ICell cell, decimal value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        public static void SetCellValue(this ICell cell, object value)
        {
            var valueType = value.GetType();

            if (valueType == typeof(bool))
            {
                cell.SetCellValue(Convert.ToBoolean(value));
                return;
            }

            if (valueType == typeof(DateTime))
            {
                cell.SetCellValue(Convert.ToDateTime(value));
                return;
            }

            if (valueType == typeof(decimal))
            {
                cell.SetCellValue(Convert.ToDecimal(value));
                return;
            }

            if (valueType == typeof(double))
            {
                cell.SetCellValue(Convert.ToDouble(value));
                return;
            }

            if (valueType == typeof(int))
            {
                cell.SetCellValue(Convert.ToInt32(value));
                return;
            }

            if (valueType == typeof(long))
            {
                cell.SetCellValue(Convert.ToInt64(value));
                return;
            }

            if (valueType == typeof(string))
            {
                cell.SetCellValue(Convert.ToString(value));
            }
        }

        public static object Value<T>(this T source, string propertyName)
        {
            var propertyInfo = source.GetType().GetProperty(propertyName);
            var value = propertyInfo?.GetValue(source);
            return value;
        }
    }
}