using System.Linq;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class PercentFormatPropertyExcelHandler : PropertyHandler<PercentFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(PercentFormatProperty property, ExcelReportCell cell)
        {
            if (!(property.PostfixText ?? string.Empty).Contains('%'))
            {
                cell.Value = cell.GetNullableValue<decimal>() * 100;
            }

            // if postfix text is ' percents (%) here', then it should be converted to '" percents ("%") here"'
            string postfix = $"\"{property.PostfixText}\""

                // surround percent sign with double quotes so it is not part of format string
                // and can be treated correctly by office
                .Replace("%", "\"%\"");

            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}{postfix}";
        }
    }
}
