using XReports.Helpers;

namespace XReports.Models
{
    public sealed class RowId
    {
        public RowId(string value)
        {
            Validation.NotNull(nameof(value), value);

            this.Value = value;
        }

        public string Value { get; }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj)
                || (obj is RowId other && this.Equals(other));
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        private bool Equals(RowId other)
        {
            return this.Value == other.Value;
        }
    }
}
