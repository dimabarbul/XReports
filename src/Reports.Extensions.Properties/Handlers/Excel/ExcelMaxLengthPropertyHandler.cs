using Reports.Excel.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.Handlers.Excel
{
    public class ExcelMaxLengthPropertyHandler : SingleTypePropertyHandler<MaxLengthProperty, ExcelReportCell>
    {
        protected override void HandleProperty(MaxLengthProperty property, ExcelReportCell cell)
        {
            string text = cell.GetValue<string>();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (text.Length <= property.MaxLength)
            {
                return;
            }

            cell.InternalValue = text.Substring(0, property.MaxLength - 1) + "…";
        }
    }
}