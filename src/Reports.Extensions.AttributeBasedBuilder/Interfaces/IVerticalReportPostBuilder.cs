using Reports.Core.SchemaBuilders;

namespace Reports.Extensions.AttributeBasedBuilder.Interfaces
{
    public interface IVerticalReportPostBuilder<TSourceEntity>
    {
        void Build(VerticalReportSchemaBuilder<TSourceEntity> builder);
    }
}
