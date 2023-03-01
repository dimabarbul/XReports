using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;

namespace XReports.AttributeHandlers
{
    public class CommonAttributeHandler : AttributeHandler<BasePropertyAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
            BasePropertyAttribute attribute)
        {
            ReportCellProperty property = this.GetCellProperty(attribute);
            if (property == null)
            {
                return;
            }

            if (attribute.IsHeader)
            {
                cellsProviderBuilder.AddHeaderProperties(property);
            }
            else
            {
                cellsProviderBuilder.AddProperties(property);
            }
        }

        private ReportCellProperty GetCellProperty(BasePropertyAttribute attribute)
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
                case DecimalPrecisionAttribute decimalPrecisionAttribute:
                    property = new DecimalPrecisionProperty(decimalPrecisionAttribute.Precision, decimalPrecisionAttribute.PreserveTrailingZeros);
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
