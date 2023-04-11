using XReports.Helpers;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Identifier of report column.
    /// </summary>
    public sealed class ColumnId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnId" /> class.
        /// </summary>
        /// <param name="value">Column identifier value.</param>
        public ColumnId(string value)
        {
            Validation.NotNull(nameof(value), value);

            this.Value = value;
        }

        /// <summary>
        /// Gets column identifier value.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj)
                || (obj is ColumnId other && this.Equals(other));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        private bool Equals(ColumnId other)
        {
            return this.Value == other.Value;
        }
    }
}
