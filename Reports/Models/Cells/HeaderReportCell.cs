using Reports.Interfaces;

namespace Reports.Models.Cells
{
    public class HeaderReportCell : IReportCell
    {
        public string DisplayValue { get; }

        public HeaderReportCell(string text)
        {
            this.DisplayValue = text;
        }
    }
}
