using Reports.Interfaces;

namespace Reports.Models.Cells
{
    public class TextReportCell : IReportCell
    {
        public string DisplayValue { get; }

        public TextReportCell(string text)
        {
            this.DisplayValue = text;
        }
    }
}
