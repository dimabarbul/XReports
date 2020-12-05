using Reports.Core.SchemaBuilders;

namespace Reports.Extensions.AttributeBasedBuilder.Interfaces
{
    public interface IHorizontalReportPostBuilder<TSourceEntity>
    {
        void Build(HorizontalReportSchemaBuilder<TSourceEntity> builder);
    }
}
