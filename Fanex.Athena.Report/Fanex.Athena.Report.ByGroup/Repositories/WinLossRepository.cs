using Fanex.Athena.Report.ByGroup.Consts;
using Fanex.Athena.Report.ByGroup.DTO;
using Fanex.Athena.Report.ByGroup.MetaModels;
using Fanex.Athena.Report.Common.Repositories;
using Fanex.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.ByGroup.Repositories
{
    public class WinLossRepository : BaseRepository, IWinLossRepository
    {
        private static readonly IList<string> WinLossDataItemIgnoreProperties = new List<string> { "CurrencyCode" };

        public async Task<(IEnumerable<WinLossDataItemDTO>, WinLossDataTotalDTO, int)> GetData(WinLossMetaModel metaModel)
        {
            using (var db = ObjectDbFactory.CreateInstance(SpConsts.WinLossReport))
            {
                var (dataItems, dataTotal) = await GetDataSet(metaModel, db);
                var totalRows = GetTotalRows(db);
                return (dataItems, dataTotal, totalRows);
            }
        }

        private async Task<(IEnumerable<WinLossDataItemDTO>, WinLossDataTotalDTO)> GetDataSet(WinLossMetaModel metaModel, IObjectDb db)
        {
            using (var reader = await db.ExecuteReaderAsync(metaModel))
            {
                var dataItems = GetDataItems<WinLossDataItemDTO>(reader);
                var dataTotal = GetDataTotal<WinLossDataTotalDTO>(reader);
                return (dataItems, dataTotal);
            }
        }

        protected override object GetValueColumn<T>(IDataReader reader, PropertyInfo property)
        {
            if (typeof(T) == typeof(WinLossDataItemDTO) && WinLossDataItemIsPropertyIgnore(property.Name))
            {
                return null;
            }

            return base.GetValueColumn<T>(reader, property);
        }

        private static bool WinLossDataItemIsPropertyIgnore(string propertyName)
        {
            return WinLossDataItemIgnoreProperties.Any(_ =>
                _.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}