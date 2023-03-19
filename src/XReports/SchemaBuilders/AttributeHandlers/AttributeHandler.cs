using System;

namespace XReports.SchemaBuilders.AttributeHandlers
{
    public abstract class AttributeHandler<TAttribute> : IAttributeHandler
        where TAttribute : Attribute
    {
        public void Handle<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> schemaBuilder,
            IReportColumnBuilder<TSourceEntity> columnBuilder,
            Attribute attribute)
        {
            if (attribute is TAttribute typedAttribute)
            {
                this.HandleAttribute(schemaBuilder, columnBuilder, typedAttribute);
            }
        }

        protected abstract void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportColumnBuilder<TSourceEntity> columnBuilder,
            TAttribute attribute);
    }
}
