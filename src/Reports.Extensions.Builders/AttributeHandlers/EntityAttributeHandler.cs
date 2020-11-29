using System;
using Reports.Extensions.Builders.Interfaces;
using Reports.SchemaBuilders;

namespace Reports.Extensions.Builders.AttributeHandlers
{
    public abstract class EntityAttributeHandler<TAttribute> : IEntityAttributeHandler
        where TAttribute : Attribute
    {
        public void Handle<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, Attribute attribute)
        {
            if (attribute is TAttribute)
            {
                this.HandleAttribute(builder, attribute);
            }
        }

        protected abstract void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, Attribute attribute);
    }
}
