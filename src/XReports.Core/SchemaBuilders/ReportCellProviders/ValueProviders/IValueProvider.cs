namespace XReports.SchemaBuilders.ReportCellProviders.ValueProviders
{
    /// <summary>
    /// Interface for value providers that can be used in <see cref="ValueProviderReportCellProvider{TSourceEntity,TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">Type of value to provide.</typeparam>
    public interface IValueProvider<out TValue>
    {
        /// <summary>
        /// Returns value.
        /// </summary>
        /// <returns>Value.</returns>
        TValue GetValue();
    }
}
