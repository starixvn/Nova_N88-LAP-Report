using Fanex.Athena.Report.Common.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Report.Common.Services.Interfaces
{
    public interface IProductService
    {
        Task<IDictionary<int, TpProduct>> GetDictionaryTpProductAsync();
    }
}