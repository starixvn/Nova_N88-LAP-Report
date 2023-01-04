using System.Threading.Tasks;
using Fanex.Athena.Report.AbnormalBet.MetaModels;

namespace Fanex.Athena.Report.AbnormalBet.Services.Interfaces
{
    public interface IAbnormalBetService
    {
        Task<dynamic> GetDataAsync(AbnormalBetMetaModel metaModel);
    }
}