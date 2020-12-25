using System;
using XReports.SchemaBuilders;
using XReports.Interfaces;

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