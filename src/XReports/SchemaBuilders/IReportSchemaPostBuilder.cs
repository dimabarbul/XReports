namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Interface for post-builder of report schema.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public interface IReportSchemaPostBuilder<TSourceItem>
    {
        /// <summary>
        /// Builds report schema.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="options">Build options that can be used to change final report schema.</param>
        void Build(IReportSchemaBuilder<TSourceItem> builder, BuildOptions options);
    }

    /// <summary>
    /// Interface for parameterized post-builder of report schema.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    /// <typeparam name="TBuildParameter">Type of build parameter.</typeparam>
    public interface IReportSchemaPostBuilder<TSourceItem, in TBuildParameter>
    {
        /// <summary>
        /// Builds report schema.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="parameter">Build parameter.</param>
        /// <param name="options">Build options that can be used to change final report schema.</param>
        void Build(IReportSchemaBuilder<TSourceItem> builder, TBuildParameter parameter, BuildOptions options);
    }
}
