using System;
using XReports.ReportCellProperties;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

namespace XReports.SchemaBuilders.AttributeHandlers
{
    /// <summary>
    /// Handler of report cell properties defined in XReports library.
    /// </summary>
    public class CommonAttributeHandler : IAttributeHandler
    {
        /// <inheritdoc />
        public void Handle<TSourceItem>(IReportSchemaBuilder<TSourceItem> schemaBuilder, IReportColumnBuilder<TSourceItem> columnBuilder, Attribute attribute)
        {
            ReportCellProperty property = GetCellProperty(attribute);
            if (property == null)
            {
                return;
            }

            if (attribute is BasePropertyAttribute basePropertyAttribute
                && basePropertyAttribute.IsHeader)
            {
                columnBuilder.AddHeaderProperties(property);
            }
            else
            {
                columnBuilder.AddProperties(property);
            }
        }

        private static ReportCellProperty GetCellProperty(Attribute attribute)
        {
            ReportCellProperty property = null;

            switch (attribute)
            {
                case AlignmentAttribute alignmentAttribute:
                    property = new AlignmentProperty(alignmentAttribute.Alignment);
                    break;
                case BoldAttribute _:
                    property = new BoldProperty();
                    break;
                case ColorAttribute colorAttribute:
                    property = new ColorProperty(colorAttribute.FontColor, colorAttribute.BackgroundColor);
                    break;
                case DateTimeFormatAttribute dateTimeFormatAttribute:
                    property = new DateTimeFormatProperty(dateTimeFormatAttribute.Format);
                    break;
                case ExcelDateTimeFormatAttribute excelDateTimeFormatAttribute:
                    property = new ExcelDateTimeFormatProperty(
                        excelDateTimeFormatAttribute.Format,
                        excelDateTimeFormatAttribute.ExcelFormat);
                    break;
                case DecimalPrecisionAttribute decimalPrecisionAttribute:
                    property = new DecimalPrecisionProperty(
                        decimalPrecisionAttribute.Precision,
                        decimalPrecisionAttribute.PreserveTrailingZeros);
                    break;
                case MaxLengthAttribute maxLengthAttribute:
                    property = new MaxLengthProperty(maxLengthAttribute.MaxLength, maxLengthAttribute.Text);
                    break;
                case PercentFormatAttribute percentFormatAttribute:
                    property = new PercentFormatProperty(
                        percentFormatAttribute.Precision,
                        percentFormatAttribute.PostfixText,
                        percentFormatAttribute.PreserveTrailingZeros);
                    break;
                default:
                    // only predefined list of attributes is processed
                    // others will be processed separately
                    break;
            }

            return property;
        }
    }
}
