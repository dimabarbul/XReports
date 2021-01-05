namespace XReports.Interfaces
{
    public interface IAttributeBasedBuilder
    {
        IReportSchema<TEntity> BuildSchema<TEntity>();
    }
}
