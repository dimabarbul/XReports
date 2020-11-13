using Reports.Extensions.Properties.Helpers;
using Reports.Extensions.Properties.Models;
using Reports.Models;

namespace Reports.Extensions.Properties
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
