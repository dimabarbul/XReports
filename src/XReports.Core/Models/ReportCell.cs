namespace XReports.Models
{
    public class ReportCell : BaseReportCell
    {
    }

    public class ReportCell<TValue> : ReportCell
    {
        public ReportCell(TValue value)
        {
            this.Value = value;
            this.ValueType = typeof(TValue);
        }
    }
}
