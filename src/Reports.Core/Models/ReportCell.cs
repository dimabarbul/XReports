namespace Reports.Core.Models
{
    public abstract class ReportCell : BaseReportCell
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

        public override dynamic InternalValue
        {
            get => this.value;
            set => this.value = value;
        }
    }
}
