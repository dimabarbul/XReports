using System;
using Reports.Extensions.Builders.AttributeHandlers;
using Reports.Extensions.Builders.Interfaces;
using Reports.Extensions.Properties.Attributes;
using Reports.Models;
using Reports.SchemaBuilders;

namespace Reports.Extensions.Properties.AttributeHandlers
{
    public class CommonAttributeHandler : EntityAttributeHandler<AttributeBase>
    {
        protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, AttributeBase attribute)
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
                case MaxLengthAttribute maxLengthAttribute:
                    property = new MaxLengthProperty(maxLengthAttribute.MaxLength);
                    break;
                case PercentFormatAttribute percentFormatAttribute:
                    property = new PercentFormatProperty(percentFormatAttribute.Precision);
                    break;
            }

            if (property != null)
            {
                if (attribute.IsHeader)
                {
                    builder.AddHeaderProperties(property);
                }
                else
                {
                    builder.AddProperties(property);
                }
            }
        }
    }
}
