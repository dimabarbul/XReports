using System;
using Reports.SchemaBuilders;

namespace Reports.Extensions.Builders.Interfaces
{
    public interface IEntityAttributeHandler
    {
        void Handle<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, Attribute attribute);
    }
}
