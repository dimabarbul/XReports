using System;
using XReports.SchemaBuilders;

namespace XReports.Interfaces
{
    public interface IAttributeHandler
    {
        void Handle<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, Attribute attribute);
    }
}
