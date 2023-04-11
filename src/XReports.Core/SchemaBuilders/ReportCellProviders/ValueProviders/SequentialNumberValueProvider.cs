namespace XReports.SchemaBuilders.ReportCellProviders.ValueProviders
{
    /// <summary>
    /// Provider of sequential numbers.
    /// </summary>
    public class SequentialNumberValueProvider : IValueProvider<int>
    {
        private int currentNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialNumberValueProvider"/> class.
        /// </summary>
        /// <param name="startNumber">The first number to return.</param>
        public SequentialNumberValueProvider(int startNumber = 1)
        {
            this.currentNumber = startNumber;
        }

        /// <inheritdoc />
        public int GetValue()
        {
            return this.currentNumber++;
        }
    }
}
