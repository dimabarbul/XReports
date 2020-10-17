namespace Reports.Models.Cells
{
    public class HeaderReportCell : ReportCell<string>
    {
        public override bool IsHeader => true;

        public HeaderReportCell(string text)
            : base(text)
        {
        }
    }
}
