namespace XReports.Models
{
    public class ReportCell : BaseReportCell
    {
        public static readonly ReportCell EmptyCell = FromValue(string.Empty);

        public static ReportCell FromValue<TValue>(TValue value)
        {
            ReportCell cell = new ReportCell();
            cell.SetValue(value);

            return cell;
        }
    }
}
