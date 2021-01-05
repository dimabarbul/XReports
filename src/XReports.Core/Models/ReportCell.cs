namespace XReports.Models
{
    public class ReportCell : BaseReportCell
    {
    }

    public class ReportCell<TValue> : ReportCell
    {
        private TValue value;

        public ReportCell(TValue value)
        {
            this.value = value;
            this.ValueType = typeof(TValue);
        }

        public override dynamic Value
        {
            get => this.value;
            set => this.value = value;
        }
    }
}
