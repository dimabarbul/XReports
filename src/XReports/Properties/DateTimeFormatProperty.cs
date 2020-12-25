using XReports.Helpers;
using XReports.Models;

namespace XReports.Properties
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
