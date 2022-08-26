using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class PercentFormatPropertyExcelHandler : PropertyHandler<PercentFormatProperty, ExcelReportCell>
    {
        private readonly Dictionary<PercentFormatProperty, string> formatCache = new Dictionary<PercentFormatProperty, string>();

        protected override void HandleProperty(PercentFormatProperty property, ExcelReportCell cell)
        {
            // if format contains percent sign (%), the value will be automatically
            // multiplied by xlsx viewer, otherwise we need to do it by ourselves
            if (property.PostfixText == null || !property.PostfixText.Contains('%'))
            {
                cell.SetValue(cell.GetNullableValue<decimal>() * 100);
            }

            cell.NumberFormat = this.GetFormat(property);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetFormat(PercentFormatProperty property)
        {
            if (!this.formatCache.ContainsKey(property))
            {
                // if postfix text is ' percents (%) here', then it should be converted to '" percents ("%") here"'
                string postfix = $"\"{property.PostfixText}\""

                    // surround percent sign with double quotes so it is not part of format string
                    // and can be treated correctly by office
                    .Replace("%", "\"%\"");

                this.formatCache[property] = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}{postfix}";
            }

            return this.formatCache[property];
        }
    }
}
