namespace XReports.Interfaces
{
    public interface IAttributeBasedBuilder
    {
        IReportSchema<TEntity> BuildSchema<TEntity>();
        IReportSchema<TEntity> BuildSchema<TEntity, TBuildParameter>(TBuildParameter parameter);
    }
}
