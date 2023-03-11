using System;
using XReports.SchemaBuilder;

namespace XReports.Interfaces
{
    public interface IAttributeHandler
    {
        void Handle<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> schemaBuilder,
            IReportColumnBuilder<TSourceEntity> cellsProviderBuilder,
            Attribute attribute);
    }
}
