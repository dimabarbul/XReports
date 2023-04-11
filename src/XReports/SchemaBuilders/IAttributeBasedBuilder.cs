using XReports.Schema;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Interface for builder of report schema based on class attributes.
    /// </summary>
    public interface IAttributeBasedBuilder
    {
        /// <summary>
        /// Builds schema based on attributes on properties of <typeparamref name="TEntity"/> class.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity to build schema from.</typeparam>
        /// <returns>Report schema.</returns>
        IReportSchema<TEntity> BuildSchema<TEntity>();

        /// <summary>
        /// Builds schema based on attributes on properties of <typeparamref name="TEntity"/> class and runtime data.
        /// </summary>
        /// <param name="parameter">Build parameter. Allows changing schema based on runtime data. Will be passed to post-builder associated with the <typeparamref name="TEntity"/>.</param>
        /// <typeparam name="TEntity">Type of entity to build schema from.</typeparam>
        /// <typeparam name="TBuildParameter">Type of build parameter.</typeparam>
        /// <returns>Report schema.</returns>
        IReportSchema<TEntity> BuildSchema<TEntity, TBuildParameter>(TBuildParameter parameter);
    }
}
