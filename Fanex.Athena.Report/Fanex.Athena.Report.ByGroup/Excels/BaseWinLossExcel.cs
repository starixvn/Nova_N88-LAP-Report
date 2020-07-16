using Fanex.Athena.Report.ByGroup.Helpers;
using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.ByGroup.ViewModels;
using Fanex.Athena.Report.Common.Excel;
using Fanex.Athena.Report.Common.Model.ViewModel;
using Fanex.Athena.Report.Resources;
using Microsoft.Extensions.FileProviders;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.Report.ByGroup.Excels
{
    public abstract class BaseWinLossExcel
    {
        protected const string TemplateSheetName = "Template";
        protected const string SheetName = "Data";
        protected const string DateFormat = "MM/dd/yyyy";

        protected const int RowTitleTemplate = 0;
        protected const int RowHeaderTemplate = 1;

        protected const int ColumnGroup = 0;
        protected const int ColumnCurrency = 1;

        protected abstract int RowParentContentTemplate { get; }
        protected abstract int RowChildContentTemplate { get; }
        protected abstract int RowNoInfoTemplate { get; }
        protected abstract int ColumnTemplateCount { get; }
        protected abstract int RowTotalDataTemplate { get; }
        protected abstract string TemplatePath { get; }

        private readonly IExcelBuilder _builder;
        private readonly IFileProvider _fileProvider;

        protected BaseWinLossExcel(IExcelBuilder builder, IFileProvider fileProvider)
        {
            _builder = builder;
            _fileProvider = fileProvider;
        }

        public IWorkbook Export(ResultModel<WinLossRecordViewModel> data, WinLossMetaModel metaModel)
        {
            var template = _fileProvider.GetFileInfo(TemplatePath).PhysicalPath;
            var workBook = _builder.LoadTemplate(template);
            var templateSheet = workBook.GetSheet(TemplateSheetName);
            var sheet = _builder.CloneSheet(workBook, templateSheet.SheetName, SheetName);

            AssignTitle(sheet, templateSheet, metaModel);
            AssignHeader(sheet, templateSheet);

            if (data.TotalRows == 0 || !data.DataList.Any())
            {
                AssignNoInfoRow(sheet, templateSheet);
            }
            else
            {
                AssignData(sheet, templateSheet, data.DataList);
                AssignTotalData(sheet, templateSheet, data.TotalData);
            }

            _builder.RemoveSheet(workBook, TemplateSheetName);

            return workBook;
        }

        protected abstract void AssignRowData(IRow row, WinLossRecordViewModel rowData);

        protected abstract void AssignHeader(IRow row);

        protected abstract void AssignTotalData(IRow row, WinLossRecordViewModel dataTotal);

        private void AssignNoInfoRow(ISheet sheet, ISheet templateSheet)
        {
            _builder.SetRowValue(sheet, templateSheet, RowNoInfoTemplate, RowParentContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        private void AssignTitle(ISheet sheet, ISheet templateSheet, WinLossMetaModel metaModel)
        {
            var title = $"{ViewResources.WinLossAnalysis}, {metaModel.FromDate.ToString(DateFormat)} - {metaModel.ToDate.ToString(DateFormat)}," +
                $" {ViewResources.DisplayCurreny}: {metaModel.BaseCurrency.ToUpperInvariant()}," +
                $" {DataHelper.GetTimezoneName(metaModel.Timezone)}";

            _builder.SetRowValue(sheet, templateSheet, sourceRowIndex: RowTitleTemplate, destRowIndex: RowTitleTemplate, columnCount: ColumnTemplateCount, value: title);

            sheet.CreateFreezePane(0, RowParentContentTemplate);
            _builder.ClearTemplate(sheet, RowParentContentTemplate);
        }

        private void AssignHeader(ISheet sheet, ISheet templateSheet)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowHeaderTemplate, destRowIndex: RowHeaderTemplate);
            AssignHeader(row);
        }

        private void AssignTotalData(ISheet sheet, ISheet templateSheet, WinLossRecordViewModel dataTotal)
        {
            var row = _builder.CopyRow(sheet, templateSheet, sourceRowIndex: RowTotalDataTemplate, destRowIndex: sheet.LastRowNum + 1);
            AssignTotalData(row, dataTotal);
        }

        private void AssignData(ISheet sheet, ISheet templateSheet, IEnumerable<WinLossRecordViewModel> data)
        {
            var currentRowIndex = 0;
            foreach (var groupData in data)
            {
                AssignRowData(sheet, templateSheet, currentRowIndex++, groupData);
                foreach (var childData in groupData.DrillDownData)
                {
                    AssignRowData(sheet, templateSheet, currentRowIndex++, childData);
                }
            }
        }

        private void AssignRowData(ISheet sheet, ISheet templateSheet, int lastRowIndex, WinLossRecordViewModel rowData)
        {
            var isParent = string.IsNullOrEmpty(rowData.Parent);
            var row = CreateRow(sheet, templateSheet, lastRowIndex, isParent);
            row.Cells[ColumnGroup].SetCellValue(isParent ? rowData.Group : rowData.Parent);
            row.Cells[ColumnCurrency].SetCellValue(isParent ? ViewResources.All : rowData.Group);
            AssignRowData(row, rowData);
        }

        private IRow CreateRow(ISheet sheet, ISheet templateSheet, int lastRowIndex, bool isParent)
        {
            var rowContentTemplate = isParent ? RowParentContentTemplate : RowChildContentTemplate;
            return _builder.CopyRow(sheet, templateSheet, rowContentTemplate, RowParentContentTemplate + lastRowIndex);
        }
    }
}