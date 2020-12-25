using System;
using Reports.SchemaBuilders;

namespace Reports.Interfaces
{
    public interface IAttributeHandler
    {
        void Handle<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, Attribute attribute);
    }
}
