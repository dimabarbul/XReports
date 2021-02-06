using XReports.Enums;

namespace XReports.Models
{
    public class DateTimeFormatPart
    {
        public DateTimeFormatPart(DateTimeFormatPartType type)
        {
            this.Type = type;
        }

        public DateTimeFormatPart(string text)
        {
            this.Text = text;
        }

        public DateTimeFormatPartType Type { get; set; }

        public string Text { get; set; }
    }
}
