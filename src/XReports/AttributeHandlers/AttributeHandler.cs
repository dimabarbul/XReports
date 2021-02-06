using System;
using XReports.Interfaces;
using XReports.SchemaBuilders;

namespace XReports.AttributeHandlers
{
    public abstract class AttributeHandler<TAttribute> : IAttributeHandler
        where TAttribute : Attribute
    {
        public void Handle<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, Attribute attribute)
        {
            if (attribute is TAttribute typedAttribute)
            {
                this.HandleAttribute(builder, typedAttribute);
            }
        }

        protected abstract void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, TAttribute attribute);
    }
}
