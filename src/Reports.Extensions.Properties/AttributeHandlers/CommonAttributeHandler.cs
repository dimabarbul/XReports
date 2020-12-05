using Reports.Core.Models;
using Reports.Core.SchemaBuilders;
using Reports.Extensions.AttributeBasedBuilder.AttributeHandlers;
using Reports.Extensions.Properties.Attributes;

namespace Reports.Extensions.Properties.AttributeHandlers
{
    public class CommonAttributeHandler : AttributeHandler<AttributeBase>
    {
        protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, AttributeBase attribute)
        {
            ReportCellProperty property = this.GetCellProperty(attribute);
            if (property == null)
            {
                return;
            }

            if (attribute.IsHeader)
            {
                builder.AddHeaderProperties(property);
            }
            else
            {
                builder.AddProperties(property);
            }
        }

        private ReportCellProperty GetCellProperty(AttributeBase attribute)
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
                case DecimalFormatAttribute decimalFormatAttribute:
                    property = new DecimalFormatProperty(decimalFormatAttribute.Precision);
                    break;
                case MaxLengthAttribute maxLengthAttribute:
                    property = new MaxLengthProperty(maxLengthAttribute.MaxLength);
                    break;
                case PercentFormatAttribute percentFormatAttribute:
                    property = new PercentFormatProperty(percentFormatAttribute.Precision)
                    {
                        PostfixText = percentFormatAttribute.PostfixText,
                    };
                    break;
            }

            return property;
        }
    }
}
