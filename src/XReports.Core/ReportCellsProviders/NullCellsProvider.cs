using System;
using XReports.Models;

namespace XReports.ReportCellsProviders
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
