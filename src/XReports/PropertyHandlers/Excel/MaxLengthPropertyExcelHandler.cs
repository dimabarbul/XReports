using System;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class MaxLengthPropertyExcelHandler : PropertyHandler<MaxLengthProperty, ExcelReportCell>
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

            cell.SetValue(string.Concat(text.AsSpan(0, property.MaxLength - 1), "…"));
        }
    }
}
