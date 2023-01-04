using System.Collections.Generic;

namespace Fanex.Athena.Report.AbnormalBet.Repositories.Interfaces
{
    public interface ISubIssueService
    {
        IDictionary<int, string> GetSubIssueList();
    }
}