using System;

namespace XReports.Models
{
    public class ReportCell : BaseReportCell
    {
        public static ReportCell FromValue<TValue>(TValue value)
        {
            ReportCell cell = new ReportCell();
            cell.SetValue(value);

            return cell;
        }
    }
}
