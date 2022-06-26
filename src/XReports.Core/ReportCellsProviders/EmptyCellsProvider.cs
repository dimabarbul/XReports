using System;
using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public class EmptyCellsProvider<TSourceEntity> : ReportCellsProvider<TSourceEntity, string>
    {
        public EmptyCellsProvider(string title)
            : base(title)
        {
        }

        public override Func<TSourceEntity, ReportCell> CellSelector => _ => ReportCell.EmptyCell;
    }
}
