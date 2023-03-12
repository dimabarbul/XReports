using XReports.Schema;

namespace XReports.SchemaBuilders
{
    public interface IAttributeBasedBuilder
    {
        IReportSchema<TEntity> BuildSchema<TEntity>();

        IReportSchema<TEntity> BuildSchema<TEntity, TBuildParameter>(TBuildParameter parameter);
    }
}
