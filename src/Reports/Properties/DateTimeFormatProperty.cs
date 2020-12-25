using Reports.Helpers;
using Reports.Models;

namespace Reports.Properties
{
    public class DateTimeFormatProperty : ReportCellProperty
    {
        public DateTimeFormatPart[] Parts { get; set; }

        public DateTimeFormatProperty(string format)
        {
            this.Parts = DateTimeParser.Parse(format);
        }
    }
}
