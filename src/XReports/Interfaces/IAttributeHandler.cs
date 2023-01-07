using System;

namespace XReports.Interfaces
{
    public interface IAttributeHandler
    {
        void Handle<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> schemaBuilder,
            IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
            Attribute attribute);
    }
}
