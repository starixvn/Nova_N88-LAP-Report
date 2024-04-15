using System.Collections.Generic;

namespace Fanex.Athena.Report.SabaWinLossAnalysis.DTOs
{
    public class TreeViewItem<TParent, TChildren>
    {
        public TParent KeyItem { get; set; }

        public IEnumerable<TChildren> Children { get; set; }
    }
}