using XReports.Helpers;

namespace XReports.SchemaBuilders
{
    public sealed class ColumnId
    {
        public ColumnId(string value)
        {
            Validation.NotNull(nameof(value), value);

            this.Value = value;
        }

        public string Value { get; }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj)
                || (obj is ColumnId other && this.Equals(other));
        }

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
