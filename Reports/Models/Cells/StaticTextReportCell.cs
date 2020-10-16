using Reports.Interfaces;

namespace Reports.Models.Cells
{
    public class StaticTextReportCell : IReportCell
    {
        public string DisplayValue { get; }

        public StaticTextReportCell(string text)
        {
            this.DisplayValue = text;
        }
    }
}
