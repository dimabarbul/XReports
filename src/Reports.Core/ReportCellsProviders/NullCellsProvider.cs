using System;
using Reports.Models;

namespace Reports.ReportCellsProviders
{
    public class NullCellsProvider<TSourceEntity> : ReportCellsProvider<TSourceEntity, string>
    {
        public NullCellsProvider(string title)
            : base(title)
        {
        }

        public override Func<TSourceEntity, ReportCell> CellSelector => _ => new ReportCell<string>(string.Empty);
    }
}
