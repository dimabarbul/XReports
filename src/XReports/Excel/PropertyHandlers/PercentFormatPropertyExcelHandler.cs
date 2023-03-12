using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    public class PercentFormatPropertyExcelHandler : PropertyHandler<PercentFormatProperty, ExcelReportCell>
    {
        private readonly Dictionary<PercentFormatProperty, string> formatCache = new Dictionary<PercentFormatProperty, string>();

        protected override void HandleProperty(PercentFormatProperty property, ExcelReportCell cell)
        {
            // Ensure the value is convertible to decimal.
            _ = cell.GetNullableValue<decimal>();

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
                // If postfix text is ' percents (%) here', then it should be converted to '" percents ("%") here"'
                // surround percent sign with double quotes so it is not part of format string
                // and can be treated correctly by office.
                // But if percent is at the beginning or at the end, we should not add empty quotes.
                // E.g., ' in %' should become '" in "%', not '" in "%""'.
                // Also if user added quotes in postfix text, they should be preserved.
                // Format ' my "percents" (%)' should be '" my "\""percents"\"" ("%")"'
                string postfix;
                if (!string.IsNullOrEmpty(property.PostfixText))
                {
                    postfix = $"\"{property.PostfixText.Replace("\"", "\"\\\"\"")}\""
                        .Replace("%", "\"%\"");

                    if (postfix.StartsWith("\"\"%", StringComparison.Ordinal))
                    {
                        postfix = postfix.Substring(2);
                    }

                    if (postfix.EndsWith("%\"\"", StringComparison.Ordinal))
                    {
                        postfix = postfix.Substring(0, postfix.Length - 2);
                    }
                }
                else
                {
                    postfix = string.Empty;
                }

                this.formatCache[property] = $"0.{string.Concat(Enumerable.Repeat(property.PreserveTrailingZeros ? '0' : '#', property.Precision))}{postfix}";
            }

            return this.formatCache[property];
        }
    }
}
