using System;

namespace XReports.SchemaBuilders
{
    public interface IAttributeHandler
    {
        void Handle<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> schemaBuilder,
            IReportColumnBuilder<TSourceEntity> cellsProviderBuilder,
            Attribute attribute);
    }
}
