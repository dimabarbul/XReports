using System;

namespace XReports.SchemaBuilders.AttributeHandlers
{
    /// <summary>
    /// Base class for attribute handlers that handle one hierarchy of attributes.
    /// </summary>
    /// <typeparam name="TAttribute">Type of attribute to handle. Derived types are handled as well.</typeparam>
    public abstract class AttributeHandler<TAttribute> : IAttributeHandler
        where TAttribute : Attribute
    {
        /// <inheritdoc />
        public void Handle<TSourceItem>(
            IReportSchemaBuilder<TSourceItem> schemaBuilder,
            IReportColumnBuilder<TSourceItem> columnBuilder,
            Attribute attribute)
        {
            if (attribute is TAttribute typedAttribute)
            {
                this.HandleAttribute(schemaBuilder, columnBuilder, typedAttribute);
            }
        }

        /// <summary>
        /// Handles attribute to specific type.
        /// </summary>
        /// <param name="schemaBuilder">Report schema builder.</param>
        /// <param name="columnBuilder">Column builder that was created from property with the attribute.</param>
        /// <param name="attribute">Attribute to handle.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        protected abstract void HandleAttribute<TSourceItem>(
            IReportSchemaBuilder<TSourceItem> schemaBuilder,
            IReportColumnBuilder<TSourceItem> columnBuilder,
            TAttribute attribute);
    }
}
