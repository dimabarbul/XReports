using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies report column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ReportColumnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportColumnAttribute"/> class.
        /// </summary>
        /// <param name="order">Report column order.</param>
        /// <param name="title">Report column title.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> is null.</exception>
        public ReportColumnAttribute(int order, string title)
        {
            this.Order = order;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        /// <summary>
        /// Gets report column order.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Gets report column title.
        /// </summary>
        public string Title { get; }
    }
}
