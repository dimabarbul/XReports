using System;
using XReports.Interfaces;
using XReports.SchemaBuilder;

namespace XReports.AttributeHandlers
{
    public abstract class AttributeHandler<TAttribute> : IAttributeHandler
        where TAttribute : Attribute
    {
        public void Handle<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> schemaBuilder,
            IReportColumnBuilder<TSourceEntity> cellsProviderBuilder,
            Attribute attribute)
        {
            if (attribute is TAttribute typedAttribute)
            {
                this.HandleAttribute(schemaBuilder, cellsProviderBuilder, typedAttribute);
            }
        }

        protected abstract void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportColumnBuilder<TSourceEntity> cellsProviderBuilder,
            TAttribute attribute);
    }
}
