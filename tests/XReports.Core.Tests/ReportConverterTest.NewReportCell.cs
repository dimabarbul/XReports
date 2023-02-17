using System.Collections.Generic;
using XReports.Models;

namespace XReports.Core.Tests
{
    public partial class ReportConverterTest
    {
        private NewReportCell CreateCell<TValue>(
            TValue value,
            int columnSpan = 1,
            int rowSpan = 1,
            IEnumerable<string> data = null,
            params ReportCellProperty[] properties)
        {
            NewReportCell cell = new NewReportCell();
            cell.SetValue(value);
            cell.ColumnSpan = columnSpan;
            cell.RowSpan = rowSpan;
            cell.AddProperties(properties);

            if (data != null)
            {
                cell.Data.AddRange(data);
            }

            return cell;
        }

        private class NewReportCell : BaseReportCell
        {
            public List<string> Data { get; private set; } = new List<string>();

            public override void Clear()
            {
                base.Clear();

                this.Data.Clear();
            }

            public override BaseReportCell Clone()
            {
                NewReportCell reportCell = (NewReportCell)base.Clone();

                reportCell.Data = new List<string>(this.Data);

                return reportCell;
            }
        }
    }
}
