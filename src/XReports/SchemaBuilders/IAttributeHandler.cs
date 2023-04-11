using System;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Interface for classes handling attributes during building of report schema by <see cref="AttributeBasedBuilder"/>.
    /// </summary>
    public interface IAttributeHandler
    {
        /// <summary>
        /// Handles attributes.
        /// </summary>
        /// <param name="schemaBuilder">Report schema builder.</param>
        /// <param name="columnBuilder">Column builder that was created from property with the attribute.</param>
        /// <param name="attribute">Attribute to handle.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        void Handle<TSourceItem>(
            IReportSchemaBuilder<TSourceItem> schemaBuilder,
            IReportColumnBuilder<TSourceItem> columnBuilder,
            Attribute attribute);
    }
}
